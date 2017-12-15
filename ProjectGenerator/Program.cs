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
        private static readonly string ProjectName = "FluentJsonNet";
        private static readonly string TestProjectName = "FluentJsonNet.Tests";

        static void Main(string[] args)
        {
            Console.WriteLine($"Environment.CurrentDirectory = {Environment.CurrentDirectory}");

            // TEST PROJECTS:
            //
            // Test projects are generated from the main multi-targeted test project:
            // "{TestProjectName}.csproj" file.

            {

                var l2jsTests = new XmlDocument();
                l2jsTests.Load($"../{TestProjectName}/{TestProjectName}.csproj");

                var targetsElement = l2jsTests.SelectNodes("//TargetFrameworks");
                Console.WriteLine(targetsElement[0].InnerText);
                var targets = targetsElement[0].InnerText.Split(";");
                foreach (var target in targets)
                {
                    if (Directory.Exists($"Templates/{target}"))
                    {
                        var dic = new Dictionary<string, object>
                        {
                            { $"TestProjectName", TestProjectName },
                            { $"TestProjectGuid", "{6F8C92C5-38F6-460E-9DDA-6334A1575B31}" },
                            { $"TestProjectRefGuid", "{2FDD0DD1-E278-4AEE-9FD1-69702458519C}" },
                            { $"CurrentYear", DateTime.Now.Year },
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
                                var targetFileName = Path.Combine(Path.GetFullPath($@"../{TestProjectName}.{target}/"), file);
                                Directory.CreateDirectory(Path.GetDirectoryName(targetFileName));
                                File.Copy(
                                    Path.Combine(Path.GetFullPath($@"../{TestProjectName}/"), file),
                                    targetFileName,
                                    true);
                            }
                    }
                    else
                    {
                        if (File.Exists($"../{TestProjectName}/{TestProjectName}.{target}.csproj"))
                            continue;

                        var l2jsTestNew = (XmlDocument)l2jsTests.Clone();
                        var node = l2jsTestNew.SelectSingleNode("//TargetFrameworks");
                        var targetElement = l2jsTestNew.CreateElement("TargetFramework");
                        targetElement.InnerText = target;
                        node.ParentNode.ReplaceChild(targetElement, node);
                        l2jsTestNew.Save($"../{TestProjectName}/{TestProjectName}.{target}.csproj");
                    }
                }

            }

            // SIGNED ASSEMBLY
            //
            // Signed assembly is generated from the main "{ProjectName}.csproj" file.

            {

                var l2js = new XmlDocument();
                l2js.Load($"../{ProjectName}/{ProjectName}.csproj");

                var sig = (XmlDocument)l2js.Clone();
                var sigMain = sig.SelectSingleNode("//TargetFrameworks").ParentNode;
                var ver = sigMain.SelectSingleNode("Version").InnerText;

                sigMain.SelectSingleNode("AssemblyVersion").InnerText = $"{ver}.0";
                sigMain.SelectSingleNode("FileVersion").InnerText = $"{ver}.0";
                sigMain.SelectSingleNode("PackageId").InnerText += ".Signed";
                sigMain.AppendChild(sig.CreateElement("SignAssembly").With(x => x.InnerText = "true"));
                sigMain.AppendChild(sig.CreateElement("AssemblyOriginatorKeyFile").With(x => x.InnerText = $@"..\{ProjectName}.snk"));

                if (sigMain.SelectSingleNode("RootNamespace") == null)
                    sigMain.AppendChild(sig.CreateElement("RootNamespace").With(x => x.InnerText = $"{ProjectName}"));

                if (sigMain.SelectSingleNode("AssemblyName") == null)
                    sigMain.AppendChild(sig.CreateElement("AssemblyName").With(x => x.InnerText = $"{ProjectName}.Signed"));
                else
                    sigMain.SelectSingleNode("AssemblyName").InnerText += ".Signed";

                foreach (XmlNode doc in sig.SelectNodes("//DocumentationFile"))
                    doc.InnerText = doc.InnerText.Replace($"{ProjectName}.xml", $"{ProjectName}.Signed.xml");

                sig.Save($"../{ProjectName}/{ProjectName}.Signed.csproj");

            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
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
}
