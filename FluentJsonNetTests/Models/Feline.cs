namespace FluentJsonNetTests.Models
{
    public abstract class Feline : Animal
    {
        protected Feline()
        {
        }

        protected Feline(float speed, float sightRange) : base(speed)
        {
            this.SightRange = sightRange;
        }

        public float SightRange { get; set; }
    }
}