using FluentJsonNet;
using FluentJsonNetTests.Models.Controls;

namespace FluentJsonNetTests.Maps.Controls
{
    [NonDefaultTestMapper]
    public class EditorModelMapNoDiscriminator : JsonSubclassMap<EditorModel>
    {
        public EditorModelMapNoDiscriminator()
        {
            this.Map(x => x.Default, "default");
        }
    }
}