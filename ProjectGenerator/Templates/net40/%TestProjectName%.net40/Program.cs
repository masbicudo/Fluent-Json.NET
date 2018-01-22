using %TestProjectName%;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace %TestProjectName%.net40
{
    class Program
    {
        static Type projMeta = typeof(%TestProjectName%.ProjectMetadata);

        static void Main(string[] args)
        {
            Assert.Default = new MyAssert();

            var failedList = new List<MethodInfo>();

            var allTestClasses = projMeta.Assembly.GetTypes()
                .Where(t => t.GetCustomAttributes<TestClassAttribute>().Any())
                .ToArray();

            var allTestMethods = allTestClasses
                .SelectMany(t => t.GetMethods()
                    .Where(m => m.GetCustomAttributes<TestMethodAttribute>().Any())
                )
                .ToArray();

            while (true)
            {
                var testsToRun = new MethodInfo[0];

                var testFilters = new List<string>();
                var all = false;
                var err = false;
                if (failedList.Count == 0) all = true;
                else err = true;
                Type type = null;
                MethodInfo method = null;

                while (true)
                {
                    if (all) testsToRun = allTestMethods;
                    if (err) testsToRun = failedList.ToArray();
                    if (type != null) testsToRun = testsToRun.Where(mi => mi.DeclaringType == type).ToArray();
                    if (method != null) testsToRun = new[] { method };

                    var prev = Console.ForegroundColor;

                    Console.Clear();
                    Console.WriteLine("Tests with .NET 4.0");
                    Console.WriteLine("===================");
                    Console.WriteLine("");
                    Console.WriteLine("This tool will run all the tests using the .Net Framework 4.0");
                    Console.WriteLine("It is needed because I could not find the usual test classes");
                    Console.WriteLine("in this version of the framework.");
                    Console.WriteLine("");
                    Console.WriteLine("===================");
                    Console.WriteLine("");

                    Console.WriteLine($" Tests selected: {testsToRun.Length} ");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" A ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" Select all tests ");
                    Console.ForegroundColor = prev;

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" C ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" Select tests in class... ");
                    Console.ForegroundColor = prev;

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" M ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" Select specific test method... ");
                    Console.ForegroundColor = prev;

                    Console.ForegroundColor = failedList.Count > 0 ? ConsoleColor.White : ConsoleColor.DarkGray;
                    Console.Write(" E ");
                    Console.ForegroundColor = failedList.Count > 0 ? ConsoleColor.Gray : ConsoleColor.DarkGray;
                    Console.WriteLine(" Select previously failed tests ");
                    Console.ForegroundColor = prev;

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" F ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" Filter selected tests list... ");
                    Console.ForegroundColor = prev;

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" R ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" Run selected tests ");
                    Console.ForegroundColor = prev;

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" Q ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" Quit ");
                    Console.ForegroundColor = prev;

                    ConsoleKeyInfo key;
                    while (true)
                    {
                        key = Console.ReadKey(intercept: true);
                        var keys = "ACMFRQ".ToList();
                        if (failedList.Count > 0) keys.Add('E');
                        if (keys.Contains(char.ToUpperInvariant(key.KeyChar)))
                            break;
                    }

                    Console.WriteLine(char.ToUpperInvariant(key.KeyChar));

                    if (key.Key == ConsoleKey.R)
                        break;

                    switch (key.Key)
                    {
                        case ConsoleKey.Q:

                            return;

                        case ConsoleKey.A:

                            all = true;
                            err = false;
                            break;

                        case ConsoleKey.E:

                            all = false;
                            err = true;
                            break;

                        case ConsoleKey.C:

                            var typeList = allTestClasses
                                .Where(t => t.GetCustomAttributes<TestClassAttribute>().Any())
                                //.Where(t => testsToRun.Any(mi => mi.DeclaringType == t))
                                .ToArray();

                            for (var it = 0; it < typeList.Length; it++)
                            {
                                var item = typeList[it];
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($" {it + 1:####} ");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine($" {item.Name} ");
                            }

                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($" C ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine($" [Clear class filter] ");

                            Console.ForegroundColor = prev;

                            while (true)
                            {
                                var text = Console.ReadLine()
                                    .Trim()
                                    .ToUpperInvariant();

                                if (int.TryParse(text, out int num))
                                    if (num > 0 && num <= typeList.Length)
                                    {
                                        type = typeList[num - 1];
                                        break;
                                    }
                                    else if (num == 0)
                                    {
                                        type = null;
                                        break;
                                    }

                                if (text == "C" || text == "CLEAR" || text == "NULL")
                                {
                                    type = null;
                                    break;
                                }

                                Console.WriteLine("Invalid!");
                            }

                            break;

                        case ConsoleKey.M:

                            var idxm = 0;
                            var miList = new List<MethodInfo>();
                            foreach (var cls in allTestClasses)
                            {
                                var mis = cls.GetMethods()
                                    .Where(m => m.GetCustomAttributes<TestMethodAttribute>().Any())
                                    .ToArray();

                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine($" {cls.Name} ");
                                foreach (var mi in mis)
                                {
                                    miList.Add(mi);
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write($" {++idxm:####} ");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.WriteLine($" {mi.Name} ");
                                }
                            }

                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($" C ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine($" [Clear class filter] ");

                            Console.ForegroundColor = prev;

                            while (true)
                            {
                                var text2 = Console.ReadLine()
                                    .Trim()
                                    .ToUpperInvariant();

                                if (int.TryParse(text2, out int num2))
                                    if (num2 > 0 && num2 <= miList.Count)
                                    {
                                        method = miList[num2 - 1];
                                        break;
                                    }
                                    else if (num2 == 0)
                                    {
                                        method = null;
                                        break;
                                    }

                                if (text2 == "C" || text2 == "CLEAR" || text2 == "NULL")
                                {
                                    method = null;
                                    break;
                                }

                                Console.WriteLine("Invalid!");
                            }

                            break;
                    }
                }




                failedList.Clear();

                int passed = 0;
                int failed = 0;
                int total = 0;

                foreach (var t in allTestClasses)
                {
                    var methods = testsToRun
                        .Where(mi => mi.DeclaringType == t)
                        .ToArray();

                    var obj = Activator.CreateInstance(t);
                    Assert.Default.CurrentClass = t.Name;

                    {
                        var prev = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine();
                        Console.WriteLine($"{t.Name}");
                        Console.WriteLine();
                        Console.ForegroundColor = prev;
                    }

                    foreach (var mi in methods)
                    {
                        Assert.Default.CurrentMethod = mi.Name;
                        total++;
                        try
                        {
                            Assert.Default.State = TestState.Ok;
                            mi.Invoke(obj, null);
                        }
                        catch (TargetInvocationException tex)
                        {
                            var ex = tex.InnerException;
                            if (!(ex is TestEndException))
                            {
                                var ok = mi.GetCustomAttributes<ExpectedException>().Where(x => x.Accept(ex)).Any();
                                Assert.Default.State = ok ? TestState.Ok : TestState.Error;

                                if (!ok)
                                {
                                    var prev = Console.ForegroundColor;
                                    Console.ForegroundColor = ok ? ConsoleColor.Green : ConsoleColor.Red;
                                    Console.Write(ok ? "  OK  " : " FAIL ");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.Write($"{Assert.Default.CurrentMethod}");
                                    Console.ForegroundColor = prev;
                                    Console.WriteLine($" {ex.GetType().Name}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Assert.Default.State = TestState.Error;
                            var prev = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(" FAIL ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write($"{Assert.Default.CurrentMethod}");
                            Console.ForegroundColor = prev;
                            Console.WriteLine($" {ex.GetType().Name}: {ex.Message}");
                        }

                        if (Assert.Default.State == TestState.Ok)
                            passed++;

                        if (Assert.Default.State == TestState.Error)
                        {
                            failedList.Add(mi);
                            failed++;
                        }
                    }
                }

                var inconclusive = total - passed - failed;
                var prev2 = Console.ForegroundColor;
                Console.WriteLine("");
                Console.WriteLine("===================");
                Console.WriteLine("");
                Console.WriteLine("Tests finished running:");

                Console.Write("Passed: ");
                Console.ForegroundColor = passed > 0 ? ConsoleColor.Green : prev2;
                Console.WriteLine(passed);
                Console.ForegroundColor = prev2;

                Console.Write("Failed: ");
                Console.ForegroundColor = failed > 0 ? ConsoleColor.Red : prev2;
                Console.WriteLine(failed);
                Console.ForegroundColor = prev2;

                Console.Write("Inconclusive: ");
                Console.ForegroundColor = inconclusive > 0 ? ConsoleColor.Yellow : prev2;
                Console.WriteLine(inconclusive);
                Console.ForegroundColor = prev2;

                Console.Write("Total: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(total);
                Console.ForegroundColor = prev2;

                Console.WriteLine("");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(intercept: true);
            }
        }
    }
}
