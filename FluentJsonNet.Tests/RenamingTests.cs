using FluentJsonNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;

namespace FluentJsonNet.Tests
{
    [TestClass]
    public class RenamingTests
    {
        [TestInitialize]
        public void Initialize()
        {
            JsonConvert.DefaultSettings =
                JsonMaps.GetDefaultSettings(
                    this.GetType().GetTypeInfo()
                        .Assembly.GetTypes()
                        .Where(t => !t.GetTypeInfo().GetCustomAttributes(typeof(NonDefaultTestMapperAttribute), false).Any())
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