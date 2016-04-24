using FluentJsonNet;
using FluentJsonNetTests.Models;

namespace FluentJsonNetTests.Maps
{
    public class CatMap : JsonMap<Cat>
    {
        public CatMap()
        {
            this.Map(x => x.Cuteness, "cuteness");
        }
    }
}