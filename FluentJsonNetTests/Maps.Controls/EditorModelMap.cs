using FluentJsonNet;
using FluentJsonNetTests.Models.Controls;

namespace FluentJsonNetTests.Maps.Controls
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