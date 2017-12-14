using FluentJsonNet;
using FluentJsonNet.Utils;

namespace FluentJsonNet.Tests.Maps
{
    public class ComplexNamesSingleMap : JsonMap<ComplexNamesSingle>
    {
        public ComplexNamesSingleMap()
        {
            this.NamingConvention(NamingConventions.CamelCase);
        }
    }
}