namespace FluentJsonNetTests.Models
{
    public class Motorcycle : Vehicle
    {
        public Motorcycle()
        {
        }

        public Motorcycle(float speed, int wheels)
            : base(speed)
        {
            this.Wheels = wheels;
        }

        public int Wheels { get; set; }
    }
}