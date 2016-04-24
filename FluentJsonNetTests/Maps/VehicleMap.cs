using FluentJsonNet;
using FluentJsonNetTests.Models;

namespace FluentJsonNetTests.Maps
{
    public class VehicleMap : JsonMap<Vehicle>.AndSubtypes
    {
        public VehicleMap()
        {
            this.DiscriminateSubClassesOnField("class");
            this.Map(x => x.Speed, "speed");
        }
    }
}
