using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace ProjectsGenerator
{
    class Program
    {
        // Change this when a new version is made
        private static readonly string Ver = "0.2.1";

        private static readonly string ProjectName = "FluentJsonNet";
        private static readonly string TestProjectName = "FluentJsonNet.Tests";
        private static readonly Dictionary<string, JsonNetVersionInfo> JsonNetVersions = new Dictionary<string, JsonNetVersionInfo> {
            //{  "8.0.1", "" },
            {  "9.0.1", new JsonNetVersionInfo
                {
                    //Frameworks = "net20;net35;net40;net45;netstandard1.0;portable-net40+sl5+wp80+win8+wpa81;portable-net45+wp80+win8+wpa81",
                    TestProjectGuid = "{6F8C92C5-38F6-460E-9DDA-6334A1575901}",
                    TestProjectRefGuid = "{7AB12006-3677-422C-8BAA-722E9F64AF25}",
                    ProjectRefGuid = "{56760104-4F1D-45D5-ACD5-87CFE7FC49D4}",
                }
            },
            { "10.0.1", new JsonNetVersionInfo
                {
                    //Frameworks = "net20;net35;net40;net45;netstandard1.0;netstandard1.3;portable-net40+sl5+win8+wp8+wpa81;portable-net45+win8+wp8+wpa81",
                    TestProjectGuid = "{6F8C92C5-38F6-460E-9DDA-6334A1571001}",
                    TestProjectRefGuid = "{C84D2415-2167-4433-B17A-57729BF7CA18}",
                    ProjectRefGuid = "{FE194A27-8E05-46C9-AF49-2A1EA71EB978}",
                }
            },
        };

        static void Main(string[] args)
        {
            Console.WriteLine($"Environment.CurrentDirectory = {Environment.CurrentDirectory}");

            foreach (var verKv in JsonNetVersions)
            {
                var jsonNetVer = new Version(verKv.Key);
                //var frameworks = verKv.Value.Frameworks;

                if (Directory.Exists($"Templates/all"))
                {

                    var dic = new Dictionary<string, object>
                    {
                        { $"TestProjectName", TestProjectName },
                        { $"TestProjectGuid", verKv.Value.TestProjectGuid },
                        { $"TestProjectRefGuid", verKv.Value.TestProjectRefGuid },
                        { $"ProjectRefGuid", verKv.Value.ProjectRefGuid },
                        { $"ProjectName", ProjectName },
                        { $"CurrentYear", DateTime.Now.Year },
                        { $"Ver", Ver },
                        { $"VerMajor", new Version(Ver).Major },
                        { $"DepVerMajor", jsonNetVer.Major },
                        { $"DepVer", verKv.Key },
                    };

                    CreateFromTemplateFolder(dic, $"Templates/all", $"..");

                }

                // TEST PROJECTS:
                //
                // Test projects are generated from the main multi-targeted test project:
                // "{TestProjectName}.Lib_v{DepVerMajor}.csproj" file.

                {

                    var l2jsTests = new XmlDocument();
                    l2jsTests.Load($"../{TestProjectName}/{TestProjectName}.Lib_v{jsonNetVer.Major}.csproj");

                    var targetsElement = l2jsTests.SelectNodes("//TargetFrameworks");
                    Console.WriteLine(targetsElement[0].InnerText);
                    var targetsProj = targetsElement[0].InnerText.Split(";");
                    Console.WriteLine(targetsElement[0].InnerText);
                    //var targetsDep = frameworks.Split(";");
                    //var targets = targetsDep.Intersect(targetsProj).ToArray();
                    foreach (var target in targetsProj)
                    {
                        if (Directory.Exists($"Templates/{target}"))
                        {
                            var dic = new Dictionary<string, object>
                            {
                                { $"TestProjectName", TestProjectName },
                                { $"TestProjectGuid", verKv.Value.TestProjectGuid },
                                { $"TestProjectRefGuid", verKv.Value.TestProjectRefGuid },
                                { $"ProjectRefGuid", verKv.Value.ProjectRefGuid },
                                { $"ProjectName", ProjectName },
                                { $"CurrentYear", DateTime.Now.Year },
                                { $"DepVerMajor", jsonNetVer.Major },
                                { $"DepVer", verKv.Key },
                            };

                            string[] listIncludes = null;
                            if (target == "net45")
                            {
                                listIncludes = Directory.GetFiles($@"../{TestProjectName}", "*.cs", SearchOption.AllDirectories)
                                    .Select(s => Path.GetFullPath(s).Replace(Path.GetFullPath($@"../{TestProjectName}/"), $@""))
                                    .Where(s => !Regex.IsMatch(s, @"\bobj\b", RegexOptions.IgnoreCase) && !Regex.IsMatch(s, @"\bbin\b", RegexOptions.IgnoreCase))
                                    .ToArray();

                                dic["FileInclude"] = listIncludes;
                            }

                            CreateFromTemplateFolder(dic, $"Templates/{target}", $"..");

                            if (target == "net45")
                                foreach (var file in listIncludes)
                                {
                                    var targetFileName = Path.Combine(Path.GetFullPath($@"../{TestProjectName}.Lib_v{jsonNetVer.Major}.{target}/"), file);
                                    Directory.CreateDirectory(Path.GetDirectoryName(targetFileName));
                                    File.Copy(
                                        Path.Combine(Path.GetFullPath($@"../{TestProjectName}/"), file),
                                        targetFileName,
                                        true);
                                }
                        }
                        else
                        {
                            var l2jsTestNew = (XmlDocument)l2jsTests.Clone();
                            var node = l2jsTestNew.SelectSingleNode("//TargetFrameworks");
                            var targetElement = l2jsTestNew.CreateElement("TargetFramework");
                            targetElement.InnerText = target;
                            node.ParentNode.ReplaceChild(targetElement, node);

                            SaveXmlDoc(l2jsTestNew, $"../{TestProjectName}/{TestProjectName}.Lib_v{jsonNetVer.Major}.{target}.csproj");
                        }
                    }

                }

                // SIGNED ASSEMBLY
                //
                // Signed assembly is generated from the main "{ProjectName}.Lib_v{DepVerMajor}.csproj" file.

                {

                    var l2js = new XmlDocument();
                    l2js.Load($"../{ProjectName}/{ProjectName}.Lib_v{jsonNetVer.Major}.csproj");

                    var sig = (XmlDocument)l2js.Clone();
                    var sigMain = sig.SelectSingleNode("//TargetFrameworks").ParentNode;
                    var ver = sigMain.SelectSingleNode("Version").InnerText;

                    sigMain.SelectSingleNode("AssemblyVersion").InnerText = $"{ver}.0";
                    sigMain.SelectSingleNode("FileVersion").InnerText = $"{ver}.0";
                    sigMain.SelectSingleNode("PackageId").InnerText += $".Signed";
                    sigMain.AppendChild(sig.CreateElement("SignAssembly").With(x => x.InnerText = "true"));
                    sigMain.AppendChild(sig.CreateElement("AssemblyOriginatorKeyFile").With(x => x.InnerText = $@"..\{ProjectName}.snk"));

                    if (sigMain.SelectSingleNode("RootNamespace") == null)
                        sigMain.AppendChild(sig.CreateElement("RootNamespace").With(x => x.InnerText = $"{ProjectName}"));

                    if (sigMain.SelectSingleNode("AssemblyName") == null)
                        sigMain.AppendChild(sig.CreateElement("AssemblyName").With(x => x.InnerText = $"{ProjectName}.Lib_v{jsonNetVer.Major}.Signed"));
                    else
                        sigMain.SelectSingleNode("AssemblyName").InnerText += $".Signed";

                    foreach (XmlNode doc in sig.SelectNodes("//DocumentationFile"))
                        doc.InnerText = doc.InnerText.Replace($"{ProjectName}.xml", $"{ProjectName}.Lib_v{jsonNetVer.Major}.Signed.xml");

                    SaveXmlDoc(sig, $"../{ProjectName}/{ProjectName}.Lib_v{jsonNetVer.Major}.Signed.csproj");
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey(intercept: true);
        }

        private static void SaveXmlDoc(XmlDocument xmldoc, string fileName)
        {
            string oldFileText = null;
            try
            {
                oldFileText = File.ReadAllText(fileName);
            }
            catch (IOException)
            {
            }

            string newFileText;
            using (StringWriter sw = new StringWriter())
            using (XmlTextWriter xw = new XmlTextWriter(sw))
            {
                xw.Formatting = Formatting.Indented;
                xw.Indentation = 4;
                xmldoc.WriteTo(xw);
                newFileText = sw.ToString();
            }

            if (oldFileText == null || oldFileText != newFileText)
                File.WriteAllText(fileName, newFileText);
        }

        private static void CreateFromTemplateFolder(Dictionary<string, object> dic, string templatePath, string targetPath)
        {
            var dirInfo = new DirectoryInfo(templatePath);
            foreach (var fileInfo in dirInfo.GetFiles())
            {
                var contents = File.ReadAllText(fileInfo.FullName);
                contents = ReplaceTemplatePlaceholders(dic, contents);
                File.WriteAllText($"{targetPath}/{ReplaceTemplatePlaceholders(dic, fileInfo.Name)}", contents);
            }

            foreach (var subdirInfo in dirInfo.GetDirectories())
            {
                var subdirTarget = $"{targetPath}/{ReplaceTemplatePlaceholders(dic, subdirInfo.Name)}";
                Directory.CreateDirectory(subdirTarget);
                CreateFromTemplateFolder(dic, subdirInfo.FullName, subdirTarget);
            }
        }

        private static string ReplaceTemplatePlaceholders(Dictionary<string, object> dic, string text)
        {
            foreach (var kv in dic)
            {
                text = Regex.Replace(
                    text,
                    $@"%Begin:{kv.Key}%(?<SUBTEXT>.*?)%End:{kv.Key}%|%{kv.Key}%",
                    m =>
                    {
                        if (m.Groups["SUBTEXT"].Success)
                        {
                            var subresult = "";
                            var dic2 = new Dictionary<string, object>(dic);
                            if (kv.Value is IList)
                            {
                                var list = kv.Value as IList;
                                foreach (var item in list)
                                {
                                    dic2["CurrentValue"] = item;
                                    subresult += ReplaceTemplatePlaceholders(dic2, m.Groups["SUBTEXT"].Value);
                                }
                            }
                            else if (kv.Value is IDictionary<string, object>)
                            {
                                var subdic = kv.Value as IDictionary<string, object>;
                                foreach (var skv in subdic)
                                    dic2[skv.Key] = skv.Value;
                                subresult += ReplaceTemplatePlaceholders(dic2, m.Groups["SUBTEXT"].Value);
                            }
                            else
                            {
                                dic2["Value"] = kv.Value.ToString();
                                subresult += ReplaceTemplatePlaceholders(dic2, m.Groups["SUBTEXT"].Value);
                            }

                            return subresult;
                        }
                        else
                        {
                            var result = kv.Value.ToString();
                            return result;
                        }
                    },
                    RegexOptions.Singleline);
            }

            return text;
        }
    }

    static class ObjectExtensions
    {
        public static T With<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
    }

    class JsonNetVersionInfo
    {
        public string Frameworks { get; set; }
        public string ProjectRefGuid { get; set; }
        public string TestProjectGuid { get; set; }
        public string TestProjectRefGuid { get; set; }
    }
}
