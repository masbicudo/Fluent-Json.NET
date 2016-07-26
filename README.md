# Fluent-Json.NET
Fluent configurations for Json.NET, allows you to map objects,
use type discriminators, all without interfering with your data objects.
No attributes are required. You can even setup serialization of
third party library types.

# NuGet

    Install-Package Fluent-Json.NET

# Examples

These examples are extracted from test cases... with one or two lines less for simplicity.

 - Plugging fluent classes to Json.NET
 - Serializing with a discriminator field
 - Camel case naming

Plugging fluent classes to Json.NET
-----------------------------------

    JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(this.GetType().Assembly.GetTypes());

Serializing with a discriminator field
--------------------------------------

Discriminator fields provide additional data to the seriaized object,
so that the deserialization process can then tell what is the type of that object.

  - **Code**

        var jsonStr = JsonConvert.SerializeObject(new Lion(45f, 200f, 1000f));

  - **Resulting JSON**

        {
          "strength": 1000.0,
          "sight": 200.0,
          "speed": 45.0,
          "class": "lion"     // <-- this is the discriminator field
        }

  - **Mapping classes**

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

  - **Model classes**

        public class Animal
        {
            public Animal(float speed)
            {
                this.Speed = speed;
            }

            public float Speed { get; set; }
        }

        public abstract class Feline : Animal
        {
            protected Feline(float speed, float sightRange) : base(speed)
            {
                this.SightRange = sightRange;
            }

            public float SightRange { get; set; }
        }

        public class Lion : Feline
        {
            public Lion(float speed, float sightRange, float strength) : base(speed, sightRange)
            {
                this.Strength = strength;
            }

            public float Strength { get; set; }
        }
        
Camel case naming
-----------------

Naming strategies can be defined so that there is no need to map each field.
Just use the `NamingStrategy` method to define one.

  - **Code**

        var jsonStr = JsonConvert.SerializeObject(new ComplexNamesSubclass("Miguel", "Bicudo"));

  - **Resulting JSON**

        {
          "nameInSubclass": "Miguel",
          "nameInBase": "Bicudo",
          "type": "subtype1"
        }

  - **Mapping classes**

        public class ComplexNamesMap : JsonMap<ComplexNamesBase>.AndSubtypes
        {
            public ComplexNamesMap()
            {
                this.DiscriminateSubClassesOnField("type");
                this.DiscriminatorValue(this.DiscriminateType);

                this.NamingStrategy(NamingStrategies.CamelCase);
            }

            private string DiscriminateType(Type arg)
            {
                if (arg == typeof(ComplexNamesSubclass))
                    return "subtype1";
                if (arg == typeof(ComplexNamesBase))
                    return "base";
                throw new NotSupportedException();
            }
        }

  - **Model classes**

        public class ComplexNamesSubclass : ComplexNamesBase
        {
            public ComplexNamesSubclass(string nameOfTheEntity, string baseName)
                : base(baseName)
            {
                this.NameInSubclass = nameOfTheEntity;
            }

            public string NameInSubclass { get; set; }
        }

        public class ComplexNamesBase
        {
            public ComplexNamesBase(string nameOfTheEntity)
            {
                this.NameInBase = nameOfTheEntity;
            }

            public string NameInBase { get; set; }
        }
