namespace FluentJsonNet.Tests.Models
{
    public class Car : Vehicle
    {
        public Car()
        {
        }

        public Car(float speed, float size)
            : base(speed)
        {
            this.Size = size;
        }

        public float Size { get; set; }
    }
}