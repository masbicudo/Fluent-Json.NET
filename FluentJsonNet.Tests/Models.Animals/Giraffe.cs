namespace FluentJsonNet.Tests.Models
{
    public class Giraffe : Animal
    {
        public Giraffe()
            : base()
        {
        }

        public Giraffe(float speed, float height) : base(speed)
        {
            this.Height = height;
        }

        public float Height { get; set; }
    }
}