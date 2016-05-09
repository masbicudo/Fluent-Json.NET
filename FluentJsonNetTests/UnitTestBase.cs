using System.Linq;
using FluentJsonNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FluentJsonNetTests
{
    public class UnitTestBase
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
    }
}