using FluentJsonNet;
using FluentJsonNet.Tests.Models;

namespace FluentJsonNet.Tests.Maps
{
    public class GiraffeMap : JsonSubclassMap<Giraffe>
    {
        public GiraffeMap()
        {
            this.Map(x => x.Height, "Height");
        }
    }
}