using System;
using FluentJsonNet;
using FluentJsonNetTests.Models;

namespace FluentJsonNetTests.Maps
{
    public class FurnitureMap : JsonMap<Furniture>.AndSubtypes
    {
        public FurnitureMap()
        {
            this.DiscriminateSubClassesOnField("class");
            this.DiscriminatorValue(t => t.Name);
            this.Map(x => x.Cost, "cost");
            this.SubclassMap<Desk>(x => x.Height, "h");
            this.SubclassMap<Chair>(x => x.ComfortLevel, "comfort");
        }

        protected override Furniture CreateObject(string discriminatorValue)
        {
            switch (discriminatorValue)
            {
                case nameof(Chair):
                    return new Chair();
                case nameof(Desk):
                    return new Desk();
                default:
                    throw new Exception($"Unrecognized discriminator for `{nameof(Furniture)}` type.");
            }
        }
    }
}