using FluentJsonNet;
using FluentJsonNetTests.Models;

namespace FluentJsonNetTests.Maps
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
