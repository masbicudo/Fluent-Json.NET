namespace FluentJsonNetTests.Models
{
    public abstract class Vehicle
    {
        protected Vehicle()
        {
        }

        protected Vehicle(float speed)
        {
            this.Speed = speed;
        }

        public float Speed { get; set; }
    }
}
