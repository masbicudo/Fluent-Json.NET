using FluentJsonNet;
using FluentJsonNetTests.Models;

namespace FluentJsonNetTests.Maps
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
