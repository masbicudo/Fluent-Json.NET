using FluentJsonNet;
using FluentJsonNet.Tests.Models;

namespace FluentJsonNet.Tests.Maps
{
    public class LionMap : JsonSubclassMap<Lion>
    {
        public LionMap()
        {
            this.DiscriminatorValue("lion");
            this.Map(x => x.Strength, "strength");
        }
    }
}
