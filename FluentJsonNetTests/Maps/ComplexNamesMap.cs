using System;
using FluentJsonNet;
using FluentJsonNet.Utils;

namespace FluentJsonNetTests.Maps
{
    public class ComplexNamesMap : JsonMap<ComplexNamesBase>.AndSubtypes
    {
        public ComplexNamesMap()
        {
            this.DiscriminateSubClassesOnField("type");
            this.DiscriminatorValue(this.DiscriminateType);
            this.NamingStrategy(NamingStrategies.CamelCase);
        }

        private string DiscriminateType(Type arg)
        {
            if (arg == typeof(ComplexNamesSubclass))
                return "subtype1";
            if (arg == typeof(ComplexNamesBase))
                return "base";
            throw new NotImplementedException();
        }
    }
}