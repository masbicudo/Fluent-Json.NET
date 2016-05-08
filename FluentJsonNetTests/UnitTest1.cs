using System.Diagnostics;
using FluentJsonNetTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FluentJsonNetTests
{
    [TestClass]
    public class UnitTest1 : UnitTestBase
    {
        [TestMethod]
        public void TestMethod1()
        {
            var jsonStr = JsonConvert.SerializeObject(new Lion(45f, 200f, 1000f));

            Assert.AreEqual("{\"strength\":1000.0,\"sight\":200.0,\"speed\":45.0,\"class\":\"lion\"}", jsonStr);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var objAnimal = JsonConvert.DeserializeObject<Animal>("{\"strength\":1000.0,\"sight\":200.0,\"speed\":45.0,\"class\":\"lion\"}");

            Assert.IsInstanceOfType(objAnimal, typeof(Lion));
            var objLion = objAnimal as Lion;
            Debug.Assert(objLion != null, "objLion != null");
            Assert.AreEqual(45f, objLion.Speed);
            Assert.AreEqual(1000f, objLion.Strength);
            Assert.AreEqual(200f, objLion.SightRange);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var jsonStr = JsonConvert.SerializeObject(new Car(180f, 4.5f));

            Assert.AreEqual("{\"Size\":4.5,\"speed\":180.0,\"class\":\"FluentJsonNetTests.Models.Car, FluentJsonNetTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}", jsonStr);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var objVehicle = JsonConvert.DeserializeObject<Vehicle>("{\"Size\":4.5,\"speed\":180.0,\"class\":\"FluentJsonNetTests.Models.Car, FluentJsonNetTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}");

            Assert.IsInstanceOfType(objVehicle, typeof(Car));
            var objCar = objVehicle as Car;
            Debug.Assert(objCar != null, "objCar != null");
            Assert.AreEqual(180f, objCar.Speed);
            Assert.AreEqual(4.5f, objCar.Size);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var jsonStr = JsonConvert.SerializeObject(new Desk(1800f, 1.2f));

            Assert.AreEqual("{\"h\":1.2,\"cost\":1800.0,\"class\":\"Desk\"}", jsonStr);
        }

        [TestMethod]
        public void TestMethod6()
        {
            var objFurniture = JsonConvert.DeserializeObject<Furniture>("{\"h\":1.2,\"cost\":1800.0,\"class\":\"Desk\"}");

            Assert.IsInstanceOfType(objFurniture, typeof(Desk));
            var objDesk = objFurniture as Desk;
            Debug.Assert(objDesk != null, "objCar != null");
            Assert.AreEqual(1.2f, objDesk.Height);
            Assert.AreEqual(1800f, objDesk.Cost);
        }

        [TestMethod]
        public void TestMethod7()
        {
            var objFurnitures = JsonConvert.DeserializeObject<Furniture[]>("[{\"h\":1.2,\"cost\":1800.0,\"class\":\"Desk\"}]");

            Assert.IsInstanceOfType(objFurnitures[0], typeof(Desk));
            var objDesk = objFurnitures[0] as Desk;
            Debug.Assert(objDesk != null, "objCar != null");
            Assert.AreEqual(1.2f, objDesk.Height);
            Assert.AreEqual(1800f, objDesk.Cost);
        }

        [TestMethod]
        public void TestMethod8()
        {
            var objFurnitures = JsonConvert.DeserializeObject<Furniture[]>("[null]");

            Assert.AreEqual(objFurnitures[0], null);
        }
    }
}
