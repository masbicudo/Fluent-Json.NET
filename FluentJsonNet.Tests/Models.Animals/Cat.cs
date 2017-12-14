namespace FluentJsonNet.Tests.Models
{
    public class Cat : Feline
    {
        public Cat()
        {
        }

        public Cat(float speed, float sightRange, float cuteness) : base(speed, sightRange)
        {
            this.Cuteness = cuteness;
        }

        public float Cuteness { get; set; }
    }
}