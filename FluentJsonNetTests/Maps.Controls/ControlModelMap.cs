using FluentJsonNet;
using FluentJsonNetTests.Models.Controls;

namespace FluentJsonNetTests.Maps.Controls
{
    public class ControlModelMap : JsonMap<ControlModel>
    {
        public ControlModelMap()
        {
            this.Map(x => x.Name, "name");
        }
    }
}