using FluentJsonNet;
using FluentJsonNet.Tests.Models.Controls;

namespace FluentJsonNet.Tests.Maps.Controls
{
    public class ControlModelMap : JsonMap<ControlModel>
    {
        public ControlModelMap()
        {
            this.Map(x => x.Name, "name");
        }
    }
}