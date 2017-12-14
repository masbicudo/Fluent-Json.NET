using FluentJsonNet;
using FluentJsonNet.Tests.Models.Controls;

namespace FluentJsonNet.Tests.Maps.Controls
{
    [NonDefaultTestMapper]
    public class EditorModelMapDoubleDiscriminator : JsonSubclassMap<EditorModel>
    {
        public EditorModelMapDoubleDiscriminator()
        {
            this.DiscriminatorValue("editor");
            this.DiscriminateSubClassesOnField("editor");
            this.Map(x => x.Default, "default");
        }
    }
}