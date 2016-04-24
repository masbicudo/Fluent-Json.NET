using FluentJsonNet;
using FluentJsonNetTests.Models;

namespace FluentJsonNetTests.Maps
{
    public class FelineMap : JsonSubclassMap<Feline>
    {
        public FelineMap()
        {
            this.Map(x => x.SightRange, "sight");
        }
    }
}
