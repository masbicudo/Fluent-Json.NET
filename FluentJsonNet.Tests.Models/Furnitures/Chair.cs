namespace FluentJsonNet.Tests.Models
{
    public class Chair : Furniture
    {
        public Chair()
        {
        }

        public Chair(float cost, float comfortLevel)
            : base(cost)
        {
            this.ComfortLevel = comfortLevel;
        }

        public float ComfortLevel { get; set; }
    }
}