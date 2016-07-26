using FluentJsonNet;
using FluentJsonNet.Utils;

namespace FluentJsonNetTests.Maps
{
    public class ComplexNamesSingleMap : JsonMap<ComplexNamesSingle>
    {
        public ComplexNamesSingleMap()
        {
            this.NamingStrategy(NamingStrategies.CamelCase);
        }
    }
}