using FluentJsonNet;
using FluentJsonNetTests.Models.Controls;

namespace FluentJsonNetTests.Maps.Controls
{
    public class TextBoxModelMap : JsonSubclassMap<TextBoxModel>
    {
        public TextBoxModelMap()
        {
            this.DiscriminatorValue("TextBox");
            this.Map(x => x.Text, "text");
        }
    }
}