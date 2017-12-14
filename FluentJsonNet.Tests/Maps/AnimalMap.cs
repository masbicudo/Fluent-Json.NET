using FluentJsonNet;
using FluentJsonNet.Tests.Models;

namespace FluentJsonNet.Tests.Maps
{
    public class AnimalMap : JsonMap<Animal>
    {
        public AnimalMap()
        {
            this.DiscriminateSubClassesOnField("class");
            this.Map(x => x.Speed, "speed");
        }
    }
}
