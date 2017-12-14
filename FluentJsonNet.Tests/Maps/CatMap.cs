using FluentJsonNet;
using FluentJsonNet.Tests.Models;

namespace FluentJsonNet.Tests.Maps
{
    public class CatMap : JsonSubclassMap<Cat>
    {
        public CatMap()
        {
            this.Map(x => x.Cuteness, "cuteness");
        }
    }
}