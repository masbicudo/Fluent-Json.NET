using FluentJsonNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Linq;

namespace FluentJsonNetTests
{
    [TestClass]
    public class RenamingTests
    {
        [TestInitialize]
        public void Initialize()
        {
            JsonConvert.DefaultSettings =
                JsonMaps.GetDefaultSettings(
                    this.GetType()
                        .Assembly.GetTypes()
                        .Where(t => t.GetCustomAttributes(typeof(NonDefaultTestMapperAttribute), false).Length == 0)
                        .ToArray());
        }

        [TestMethod]
        public void Test_Serialize_CamelCaseNames()
        {
            var jsonStr = JsonConvert.SerializeObject(new ComplexNamesSingle("MASB"));
            Assert.AreEqual("{\"nameOfTheEntity\":\"MASB\"}", jsonStr);
        }

        [TestMethod]
        public void Test_Serialize_CamelCaseNames2()
        {
            var jsonStr = JsonConvert.SerializeObject(new ComplexNamesSubclass("Miguel", "Bicudo"));
            Assert.AreEqual("{\"nameInSubclass\":\"Miguel\",\"nameInBase\":\"Bicudo\",\"type\":\"subtype1\"}", jsonStr);
        }

        [TestMethod]
        public void Test_Serialize_CamelCaseNames3()
        {
            var jsonStr = JsonConvert.SerializeObject(new ComplexNamesBase("Bicudo"));
            Assert.AreEqual("{\"nameInBase\":\"Bicudo\",\"type\":\"base\"}", jsonStr);
        }
    }
}