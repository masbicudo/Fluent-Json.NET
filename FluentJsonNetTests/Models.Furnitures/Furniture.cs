namespace FluentJsonNetTests.Models
{
    public abstract class Furniture
    {
        protected Furniture()
        {
        }

        protected Furniture(float cost)
        {
            this.Cost = cost;
        }

        public float Cost { get; set; }
    }
}
