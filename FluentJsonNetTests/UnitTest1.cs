using FluentJsonNet;
using FluentJsonNetTests.Maps.Controls;
using FluentJsonNetTests.Models;
using FluentJsonNetTests.Models.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using static FluentJsonNetTests.MyAssert;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace FluentJsonNetTests
{
    [TestClass]
    public class UnitTest1 : UnitTestBase
    {
        [TestMethod]
        public void TestMethod1()
        {
            var jsonStr = JsonConvert.SerializeObject(new Lion(45f, 200f, 1000f));

            AreEqual("{\"strength\":1000.0,\"sight\":200.0,\"speed\":45.0,\"class\":\"lion\"}", jsonStr);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var objAnimal = JsonConvert.DeserializeObject<Animal>("{\"strength\":1000.0,\"sight\":200.0,\"speed\":45.0,\"class\":\"lion\"}");

            IsInstanceOfType(objAnimal, typeof(Lion));
            var objLion = objAnimal as Lion;
            Debug.Assert(objLion != null, "objLion != null");
            AreEqual(45f, objLion.Speed);
            AreEqual(1000f, objLion.Strength);
            AreEqual(200f, objLion.SightRange);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var jsonStr = JsonConvert.SerializeObject(new Car(180f, 4.5f));

            AreEqual("{\"Size\":4.5,\"speed\":180.0,\"class\":\"FluentJsonNetTests.Models.Car, FluentJsonNetTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}", jsonStr);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var objVehicle = JsonConvert.DeserializeObject<Vehicle>("{\"Size\":4.5,\"speed\":180.0,\"class\":\"FluentJsonNetTests.Models.Car, FluentJsonNetTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}");

            IsInstanceOfType(objVehicle, typeof(Car));
            var objCar = objVehicle as Car;
            Debug.Assert(objCar != null, "objCar != null");
            AreEqual(180f, objCar.Speed);
            AreEqual(4.5f, objCar.Size);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var jsonStr = JsonConvert.SerializeObject(new Desk(1800f, 1.2f));

            AreEqual("{\"h\":1.2,\"cost\":1800.0,\"class\":\"Desk\"}", jsonStr);
        }

        [TestMethod]
        public void TestMethod6()
        {
            var objFurniture = JsonConvert.DeserializeObject<Furniture>("{\"h\":1.2,\"cost\":1800.0,\"class\":\"Desk\"}");

            IsInstanceOfType(objFurniture, typeof(Desk));
            var objDesk = objFurniture as Desk;
            Debug.Assert(objDesk != null, "objCar != null");
            AreEqual(1.2f, objDesk.Height);
            AreEqual(1800f, objDesk.Cost);
        }

        [TestMethod]
        public void TestMethod7()
        {
            var objFurnitures = JsonConvert.DeserializeObject<Furniture[]>("[{\"h\":1.2,\"cost\":1800.0,\"class\":\"Desk\"}]");

            IsInstanceOfType(objFurnitures[0], typeof(Desk));
            var objDesk = objFurnitures[0] as Desk;
            Debug.Assert(objDesk != null, "objCar != null");
            AreEqual(1.2f, objDesk.Height);
            AreEqual(1800f, objDesk.Cost);
        }

        [TestMethod]
        public void TestMethod8()
        {
            var objFurnitures = JsonConvert.DeserializeObject<Furniture[]>("[null]");

            AreEqual(objFurnitures[0], null);
        }

        [TestMethod]
        public void TestMethod9()
        {
            var jsonTextBox = JsonConvert.SerializeObject(new TextBoxModel { Default = "A", Name = "N", Text = "T" });

            AreEqual(jsonTextBox, "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\"}");
        }

        [TestMethod]
        public void TestMethod9_1()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMap),
                    typeof(EditorModelMapNoDiscriminator),
                    typeof(TextBoxModelMap),
                });

            Throws<Exception>(
                "Discriminating value not set to any field.",
                () => JsonConvert.SerializeObject(new TextBoxModel { Default = "A", Name = "N", Text = "T" }));
        }

        [TestMethod]
        public void TestMethod9_2()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMap),
                    typeof(EditorModelMap),
                    typeof(TextBoxModelMapNoDiscriminator),
                });

            Throws<Exception>(
                "Value of discriminator field not defined by subclass map.",
                () => JsonConvert.SerializeObject(new TextBoxModel { Default = "A", Name = "N", Text = "T" }));
        }

        [TestMethod]
        public void TestMethod9_3()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMap),
                    typeof(EditorModelMapNoDiscriminator),
                    typeof(TextBoxModelMapNoDiscriminator),
                });
            var jsonTextBox = JsonConvert.SerializeObject(new TextBoxModel { Default = "A", Name = "N", Text = "T" });

            AreEqual(jsonTextBox, "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\"}");
        }

        [TestMethod]
        public void TestMethod9_4()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMapBaseDiscriminator),
                    typeof(EditorModelMapDoubleDiscriminator),
                    typeof(TextBoxModelMap),
                });
            var jsonTextBox = JsonConvert.SerializeObject(new TextBoxModel { Default = "A", Name = "N", Text = "T" });

            AreEqual(jsonTextBox, "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\",\"type\":\"editor\"}");
        }

        [TestMethod]
        public void TestMethod10()
        {
            Throws<Exception>(
                "Discriminator field not found.",
                () => JsonConvert.DeserializeObject<ControlModel>("{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\"}"));
        }

        [TestMethod]
        public void TestMethod10_1a()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMap),
                    typeof(EditorModelMapNoDiscriminator),
                    typeof(TextBoxModelMap),
                });

            var textBox = JsonConvert.DeserializeObject<TextBoxModel>(
                "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\"}");
            AreEqual("T", textBox.Text);
            AreEqual("A", textBox.Default);
            AreEqual("N", textBox.Name);
        }

        [TestMethod]
        public void TestMethod10_1b()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMap),
                    typeof(EditorModelMapNoDiscriminator),
                    typeof(TextBoxModelMap),
                });

            Throws<Exception>(
                "Cannot create object of type `FluentJsonNetTests.Models.Controls.EditorModel`",
                () => JsonConvert.DeserializeObject<EditorModel>("{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\"}"));
        }

        [TestMethod]
        public void TestMethod10_2()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMap),
                    typeof(EditorModelMap),
                    typeof(TextBoxModelMapNoDiscriminator),
                });

            Throws<Exception>(
                "Value of discriminator field not verified by any subclass map.",
                () => JsonConvert.DeserializeObject<EditorModel>("{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\"}"));
        }

        [TestMethod]
        public void TestMethod10_3()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMap),
                    typeof(EditorModelMapNoDiscriminator),
                    typeof(TextBoxModelMapNoDiscriminator),
                });
            Throws<Exception>(
                "Cannot create object of type `FluentJsonNetTests.Models.Controls.EditorModel`",
                () => JsonConvert.DeserializeObject<EditorModel>("{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\"}"));
        }

        [TestMethod]
        public void TestMethod10_4a()
        {
            var control = JsonConvert.DeserializeObject<EditorModel>("{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\"}");

            IsInstanceOfType(control, typeof(TextBoxModel));
            var textBox = (TextBoxModel)control;
            AreEqual("T", textBox.Text);
            AreEqual("A", textBox.Default);
            AreEqual("N", textBox.Name);
        }

        [TestMethod]
        public void TestMethod10_4b()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMapBaseDiscriminator),
                    typeof(EditorModelMapDoubleDiscriminator),
                    typeof(TextBoxModelMap),
                });
            var control = JsonConvert.DeserializeObject<ControlModel>(
                    "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\",\"type\":\"editor\"}");
            IsInstanceOfType(control, typeof(TextBoxModel));
            var textBox = (TextBoxModel)control;
            AreEqual("T", textBox.Text);
            AreEqual("A", textBox.Default);
            AreEqual("N", textBox.Name);
        }

        [TestMethod]
        public void TestMethod10_4c()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMapBaseDiscriminator),
                    typeof(EditorModelMapDoubleDiscriminator),
                    typeof(TextBoxModelMap),
                });
            var editor = JsonConvert.DeserializeObject<EditorModel>(
                    "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\",\"type\":\"editor\"}");
            IsInstanceOfType(editor, typeof(TextBoxModel));
            var textBox = (TextBoxModel)editor;
            AreEqual("T", textBox.Text);
            AreEqual("A", textBox.Default);
            AreEqual("N", textBox.Name);
        }

        [TestMethod]
        public void TestMethod10_4d()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMapBaseDiscriminator),
                    typeof(EditorModelMapDoubleDiscriminator),
                    typeof(TextBoxModelMap),
                });
            var textBox = JsonConvert.DeserializeObject<TextBoxModel>(
                    "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\",\"type\":\"editor\"}");
            AreEqual("T", textBox.Text);
            AreEqual("A", textBox.Default);
            AreEqual("N", textBox.Name);
        }

        [TestMethod]
        public void TestMethod10_5a()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMapBaseDiscriminator),
                    typeof(EditorModelMapDoubleDiscriminator),
                    typeof(TextBoxModelMap),
                });
            var editor = JsonConvert.DeserializeObject<EditorModel>(
                    "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\"}");
            IsInstanceOfType(editor, typeof(TextBoxModel));
            var textBox = (TextBoxModel)editor;
            AreEqual("T", textBox.Text);
            AreEqual("A", textBox.Default);
            AreEqual("N", textBox.Name);
        }

        [TestMethod]
        public void TestMethod10_5b()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMapBaseDiscriminator),
                    typeof(EditorModelMapDoubleDiscriminator),
                    typeof(TextBoxModelMap),
                });
            Throws<Exception>(
                "Value of discriminator field not verified by any subclass map.",
                () => JsonConvert.DeserializeObject<EditorModel>(
                    "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"CheckBox\"}"));
        }

        [TestMethod]
        public void TestMethod10_5c()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMapBaseDiscriminator),
                    typeof(EditorModelMapDoubleDiscriminator),
                    typeof(TextBoxModelMap),
                });
            Throws<Exception>(
                "Cannot create object of type `FluentJsonNetTests.Models.Controls.EditorModel`",
                () => JsonConvert.DeserializeObject<EditorModel>(
                    "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\"}"));
        }

        [TestMethod]
        public void TestMethod10_5d()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMapBaseDiscriminator),
                    typeof(EditorModelMapDoubleDiscriminator),
                    typeof(TextBoxModelMap),
                });
            var control = JsonConvert.DeserializeObject<ControlModel>(
                "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\",\"type\":\"editor\"}");
            IsInstanceOfType(control, typeof(TextBoxModel));
            var textBox = (TextBoxModel)control;
            AreEqual("T", textBox.Text);
            AreEqual("A", textBox.Default);
            AreEqual("N", textBox.Name);
        }

        [TestMethod]
        public void TestMethod10_5e()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMapBaseDiscriminator),
                    typeof(EditorModelMapDoubleDiscriminator),
                    typeof(TextBoxModelMap),
                });
            Throws<Exception>(
                "Cannot create object of type `FluentJsonNetTests.Models.Controls.ControlModel`",
                () => JsonConvert.DeserializeObject<ControlModel>(
                    "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\"}"));
        }

        [TestMethod]
        public void TestMethod10_5f()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMapBaseDiscriminator),
                    typeof(EditorModelMapDoubleDiscriminator),
                    typeof(TextBoxModelMap),
                });
            Throws<Exception>(
                "Cannot create object of type `FluentJsonNetTests.Models.Controls.ControlModel`",
                () => JsonConvert.DeserializeObject<ControlModel>(
                    "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\"}"));
        }

        [TestMethod]
        public void TestMethod11()
        {
            var control = JsonConvert.DeserializeObject<EditorModel>("{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\"}");

            IsInstanceOfType(control, typeof(TextBoxModel));
            var textBox = (TextBoxModel)control;
            AreEqual("T", textBox.Text);
            AreEqual("A", textBox.Default);
            AreEqual("N", textBox.Name);
        }

        [TestMethod]
        public void TestMethod12()
        {
            var control = JsonConvert.DeserializeObject<TextBoxModel>("{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\"}");

            IsInstanceOfType(control, typeof(TextBoxModel));
            var textBox = (TextBoxModel)control;
            AreEqual("T", textBox.Text);
            AreEqual("A", textBox.Default);
            AreEqual("N", textBox.Name);
        }
    }
}