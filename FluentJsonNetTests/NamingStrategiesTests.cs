using FluentJsonNet.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FluentJsonNetTests
{
    [TestClass]
    public class NamingStrategiesTests
    {
        [TestMethod]
        public void Test_NamingStrategies_Sep1()
        {
            var names = NamingStrategies.SeparateNames("MiguelAngeloSantosBicudo").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "Miguel", "Angelo", "Santos", "Bicudo" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep2()
        {
            var names = NamingStrategies.SeparateNames("Miguel Angelo Santos Bicudo").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "Miguel", "Angelo", "Santos", "Bicudo" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep3()
        {
            var names = NamingStrategies.SeparateNames("MIGUEL_ANGELO_SANTOS_BICUDO").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "MIGUEL", "ANGELO", "SANTOS", "BICUDO" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep4()
        {
            var names = NamingStrategies.SeparateNames("MIGUEL-ANGELO_SANTOS BICUDO").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "MIGUEL", "ANGELO", "SANTOS", "BICUDO" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep5()
        {
            var names = NamingStrategies.SeparateNames("miguel-angelo-santos-bicudo").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "miguel", "angelo", "santos", "bicudo" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep6()
        {
            var names = NamingStrategies.SeparateNames("miguel-angelo_santos bicudo").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "miguel", "angelo", "santos", "bicudo" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep7()
        {
            var names = NamingStrategies.SeparateNames("miguelAngelo_Santos BICUDO").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "miguel", "Angelo", "Santos", "BICUDO" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep8()
        {
            var names = NamingStrategies.SeparateNames("_miguelAngelo_Santos BICUDO ").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "miguel", "Angelo", "Santos", "BICUDO" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_Sep9()
        {
            var names = NamingStrategies.SeparateNames("_ -_miguelAngelo_ _Santos-- BICUDO ").ToArray();
            Assert.IsTrue(names.SequenceEqual(new[] { "miguel", "Angelo", "Santos", "BICUDO" }));
        }

        [TestMethod]
        public void Test_NamingStrategies_UpperCamelCase()
        {
            var name = NamingStrategies.UpperCamelCase("_ -_miguelAngelo_ _Santos-- BICUDO ");
            Assert.AreEqual("MiguelAngeloSantosBicudo", name);
        }

        [TestMethod]
        public void Test_NamingStrategies_AllUpper()
        {
            var name = NamingStrategies.AllUpperUnderscoreSeparated("_ -_miguelAngelo_ _Santos-- BICUDO ");
            Assert.AreEqual("MIGUEL_ANGELO_SANTOS_BICUDO", name);
        }

        [TestMethod]
        public void Test_NamingStrategies_AllLower()
        {
            var name = NamingStrategies.AllLowerHyphenSeparated("_ -_miguelAngelo_ _Santos-- BICUDO ");
            Assert.AreEqual("miguel-angelo-santos-bicudo", name);
        }

        [TestMethod]
        public void Test_FirstToUpper()
        {
            var name = NamingStrategies.FirstToUpperInvariant("miguel");
            Assert.AreEqual("Miguel", name);
        }

        [TestMethod]
        public void Test_FirstToUpper2()
        {
            var name = NamingStrategies.FirstToUpperInvariant("MIGUEL");
            Assert.AreEqual("Miguel", name);
        }
    }
}