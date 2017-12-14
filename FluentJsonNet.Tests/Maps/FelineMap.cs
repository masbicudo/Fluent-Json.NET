using FluentJsonNet;
using FluentJsonNet.Tests.Models;

namespace FluentJsonNet.Tests.Maps
{
    public class FelineMap : JsonSubclassMap<Feline>
    {
        public FelineMap()
        {
            this.Map(x => x.SightRange, "sight");
        }
    }
}
