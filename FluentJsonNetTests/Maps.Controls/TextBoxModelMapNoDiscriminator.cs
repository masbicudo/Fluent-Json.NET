using FluentJsonNet;
using FluentJsonNetTests.Models.Controls;

namespace FluentJsonNetTests.Maps.Controls
{
    public class TextBoxModelMapNoDiscriminator : JsonSubclassMap<TextBoxModel>
    {
        public TextBoxModelMapNoDiscriminator()
        {
            this.Map(x => x.Text, "text");
        }
    }
}