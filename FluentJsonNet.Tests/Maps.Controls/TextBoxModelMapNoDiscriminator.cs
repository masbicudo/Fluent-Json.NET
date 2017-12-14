using FluentJsonNet;
using FluentJsonNet.Tests.Models.Controls;

namespace FluentJsonNet.Tests.Maps.Controls
{
    [NonDefaultTestMapper]
    public class TextBoxModelMapNoDiscriminator : JsonSubclassMap<TextBoxModel>
    {
        public TextBoxModelMapNoDiscriminator()
        {
            this.Map(x => x.Text, "text");
        }
    }
}