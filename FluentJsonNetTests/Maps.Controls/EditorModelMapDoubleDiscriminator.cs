using FluentJsonNet;
using FluentJsonNetTests.Models.Controls;

namespace FluentJsonNetTests.Maps.Controls
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