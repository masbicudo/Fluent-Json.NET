using FluentJsonNet;
using FluentJsonNetTests.Models.Controls;

namespace FluentJsonNetTests.Maps.Controls
{
    public class ControlModelMapBaseDiscriminator : JsonMap<ControlModel>
    {
        public ControlModelMapBaseDiscriminator()
        {
            this.DiscriminateSubClassesOnField("type");
            this.Map(x => x.Name, "name");
        }
    }
}