using FluentJsonNet;
using FluentJsonNet.Tests.Models.Controls;

namespace FluentJsonNet.Tests.Maps.Controls
{
    public class EditorModelMap : JsonSubclassMap<EditorModel>
    {
        public EditorModelMap()
        {
            this.DiscriminateSubClassesOnField("editor");
            this.Map(x => x.Default, "default");
        }
    }
}