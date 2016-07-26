namespace FluentJsonNetTests.Models
{
    public class Lion : Feline
    {
        public Lion()
        {
        }

        public Lion(float speed, float sightRange, float strength) : base(speed, sightRange)
        {
            this.Strength = strength;
        }

        public float Strength { get; set; }
    }
}