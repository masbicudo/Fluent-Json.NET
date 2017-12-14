using FluentJsonNet;
using FluentJsonNet.Tests.Models.Controls;

namespace FluentJsonNet.Tests.Maps.Controls
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