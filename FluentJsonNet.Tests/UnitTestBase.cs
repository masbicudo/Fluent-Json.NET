using System.Linq;
using System.Reflection;
using FluentJsonNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FluentJsonNet.Tests
{
    public class UnitTestBase
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
    }
}