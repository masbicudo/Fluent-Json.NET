using FluentJsonNet;
using FluentJsonNet.Tests.Models;

namespace FluentJsonNet.Tests.Maps
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
