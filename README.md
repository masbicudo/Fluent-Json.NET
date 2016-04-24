# Fluent-Json.NET
Fluent configurations for Json.NET, allows you to map objects, use type discriminators, all without interfering with your data objects. No attributes are required.

# NuGet

    Install-Package Fluent-Json.NET

# Example

**serializing a Lion class that inherits from Animal, with a discriminator field**

    {
	    var jsonStr = JsonConvert.SerializeObject(new Lion(45f, 200f, 1000f));
	    Assert.AreEqual("{\"strength\":1000.0,\"sight\":200.0,\"speed\":45.0,\"class\":\"lion\"}", jsonStr);
    }

**Plugging fluent classes to Json.NET**

    {
        JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(this.GetType().Assembly.GetTypes());
    }

**Model classes**

    public class Animal
    {
        public Animal()
        {
        }

        public Animal(float speed)
        {
            this.Speed = speed;
        }

        public float Speed { get; set; }
    }

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

**Mapping classes**

    public class AnimalMap : JsonMap<Animal>
    {
        public AnimalMap()
        {
            this.DiscriminateSubClassesOnField("class");
            this.Map(x => x.Speed, "speed");
        }
    }

    public class FelineMap : JsonSubclassMap<Feline>
    {
        public FelineMap()
        {
            this.Map(x => x.SightRange, "sight");
        }
    }

    public class LionMap : JsonSubclassMap<Lion>
    {
        public LionMap()
        {
            this.DiscriminatorValue("lion");
            this.Map(x => x.Strength, "strength");
        }
    }
