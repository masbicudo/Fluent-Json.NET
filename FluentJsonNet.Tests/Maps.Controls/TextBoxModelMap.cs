using FluentJsonNet;
using FluentJsonNet.Tests.Models.Controls;

namespace FluentJsonNet.Tests.Maps.Controls
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