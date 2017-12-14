using FluentJsonNet.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FluentJsonNet.Tests
{
    [TestClass]
    public class NamingStrategiesTests
    {
        [TestMethod]
        public void Test_NamingStrategies_Sep1()
        {
            var names = NamingConventions.SeparateNames("MiguelAngeloSantosBicudo").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "Miguel", "Angelo", "Santos", "Bicudo" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep2()
        {
            var names = NamingConventions.SeparateNames("Miguel Angelo Santos Bicudo").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "Miguel", "Angelo", "Santos", "Bicudo" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep3()
        {
            var names = NamingConventions.SeparateNames("MIGUEL_ANGELO_SANTOS_BICUDO").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "MIGUEL", "ANGELO", "SANTOS", "BICUDO" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep4()
        {
            var names = NamingConventions.SeparateNames("MIGUEL-ANGELO_SANTOS BICUDO").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "MIGUEL", "ANGELO", "SANTOS", "BICUDO" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep5()
        {
            var names = NamingConventions.SeparateNames("miguel-angelo-santos-bicudo").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "miguel", "angelo", "santos", "bicudo" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep6()
        {
            var names = NamingConventions.SeparateNames("miguel-angelo_santos bicudo").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "miguel", "angelo", "santos", "bicudo" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep7()
        {
            var names = NamingConventions.SeparateNames("miguelAngelo_Santos BICUDO").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "miguel", "Angelo", "Santos", "BICUDO" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep8()
        {
            var names = NamingConventions.SeparateNames("_miguelAngelo_Santos BICUDO ").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "miguel", "Angelo", "Santos", "BICUDO" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep9()
        {
            var names = NamingConventions.SeparateNames("_ -_miguelAngelo_ _Santos-- BICUDO ").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "miguel", "Angelo", "Santos", "BICUDO" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_UpperCamelCase()
        {
            var name = NamingConventions.UpperCamelCase("_ -_miguelAngelo_ _Santos-- BICUDO ");
            Assert.AreEqual("MiguelAngeloSantosBicudo", name);
        }

        [TestMethod]
        public void Test_NamingStrategies_AllUpper()
        {
            var name = NamingConventions.AllUpperUnderscoreSeparated("_ -_miguelAngelo_ _Santos-- BICUDO ");
            Assert.AreEqual("MIGUEL_ANGELO_SANTOS_BICUDO", name);
        }

        [TestMethod]
        public void Test_NamingStrategies_AllLower()
        {
            var name = NamingConventions.AllLowerHyphenSeparated("_ -_miguelAngelo_ _Santos-- BICUDO ");
            Assert.AreEqual("miguel-angelo-santos-bicudo", name);
        }

        [TestMethod]
        public void Test_FirstToUpper()
        {
            var name = NamingConventions.FirstToUpperInvariant("miguel");
            Assert.AreEqual("Miguel", name);
        }

        [TestMethod]
        public void Test_FirstToUpper2()
        {
            var name = NamingConventions.FirstToUpperInvariant("MIGUEL");
            Assert.AreEqual("Miguel", name);
        }
    }
}