using FluentJsonNet;
using FluentJsonNetTests.Models.Controls;

namespace FluentJsonNetTests.Maps.Controls
{
    [NonDefaultTestMapper]
    public class ControlModelMapBaseDiscriminator : JsonMap<ControlModel>
    {
        public ControlModelMapBaseDiscriminator()
        {
            this.DiscriminateSubClassesOnField("type");
            this.Map(x => x.Name, "name");
        }
    }
}