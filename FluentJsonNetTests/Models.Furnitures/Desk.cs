namespace FluentJsonNetTests.Models
{
    public class Desk : Furniture
    {
        public Desk()
        {
        }

        public Desk(float cost, float height)
            : base(cost)
        {
            this.Height = height;
        }

        public float Height { get; set; }
    }
}