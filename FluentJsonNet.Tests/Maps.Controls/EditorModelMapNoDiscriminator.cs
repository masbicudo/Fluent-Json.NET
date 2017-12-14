using FluentJsonNet;
using FluentJsonNet.Tests.Models.Controls;

namespace FluentJsonNet.Tests.Maps.Controls
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