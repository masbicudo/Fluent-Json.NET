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
    public class DiscriminatorTests : UnitTestBase
    {
        [TestMethod]
        public void Test_Serialize_Subclass_With_Discriminator()
        {
            var jsonStr = JsonConvert.SerializeObject(new Lion(45f, 200f, 1000f));

            AreEqual("{\"strength\":1000.0,\"sight\":200.0,\"speed\":45.0,\"class\":\"lion\"}", jsonStr);
        }

        [TestMethod]
        public void Test_Deserialize_Subclass_With_Discriminator()
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
        public void Test_Deserialize_Subclass_Without_Discriminator()
        {
            var objLion = JsonConvert.DeserializeObject<Lion>("{\"strength\":1000.0,\"sight\":200.0,\"speed\":45.0}");

            Debug.Assert(objLion != null, "objLion != null");
            AreEqual(45f, objLion.Speed);
            AreEqual(1000f, objLion.Strength);
            AreEqual(200f, objLion.SightRange);
        }

        [TestMethod]
        public void Test_Serialize_IAndSubtypes_With_Default_Discriminator()
        {
            var jsonStr = JsonConvert.SerializeObject(new Car(180f, 4.5f));

            AreEqual("{\"Size\":4.5,\"speed\":180.0,\"class\":\"FluentJsonNetTests.Models.Car, FluentJsonNetTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}", jsonStr);
        }

        [TestMethod]
        public void Test_Deserialize_IAndSubtypes_With_Default_Discriminator()
        {
            var objVehicle = JsonConvert.DeserializeObject<Vehicle>("{\"Size\":4.5,\"speed\":180.0,\"class\":\"FluentJsonNetTests.Models.Car, FluentJsonNetTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}");

            IsInstanceOfType(objVehicle, typeof(Car));
            var objCar = objVehicle as Car;
            Debug.Assert(objCar != null, "objCar != null");
            AreEqual(180f, objCar.Speed);
            AreEqual(4.5f, objCar.Size);
        }

        [TestMethod]
        public void Test_Serialize_IAndSubtypes_With_Custom_Discriminator()
        {
            var jsonStr = JsonConvert.SerializeObject(new Desk(1800f, 1.2f));

            AreEqual("{\"h\":1.2,\"cost\":1800.0,\"class\":\"Desk\"}", jsonStr);
        }

        [TestMethod]
        public void Test_Serialize_IAndSubtypes_With_Custom_Discriminator_2()
        {
            var jsonStr = JsonConvert.SerializeObject(new Chair(360f, 0.8f));

            AreEqual("{\"comfort\":0.8,\"cost\":360.0,\"class\":\"Chair\"}", jsonStr);
        }

        [TestMethod]
        public void Test_Deserialize_IAndSubtypes_With_Custom_Discriminator()
        {
            var objFurniture = JsonConvert.DeserializeObject<Furniture>("{\"h\":1.2,\"cost\":1800.0,\"class\":\"Desk\"}");

            IsInstanceOfType(objFurniture, typeof(Desk));
            var objDesk = objFurniture as Desk;
            Debug.Assert(objDesk != null, "objDesk != null");
            AreEqual(1.2f, objDesk.Height);
            AreEqual(1800f, objDesk.Cost);
        }

        [TestMethod]
        public void Test_Deserialize_IAndSubtypes_With_Custom_Discriminator_SpecifyingGenericType()
        {
            var objChair = JsonConvert.DeserializeObject<Chair>("{\"comfort\":0.8,\"cost\":360.0,\"class\":\"Chair\"}");

            Debug.Assert(objChair != null, "objChair != null");
            AreEqual(0.8f, objChair.ComfortLevel);
            AreEqual(360f, objChair.Cost);
        }

        [TestMethod]
        public void Test_Deserialize_IAndSubtypes_Without_Discriminator_SpecifyingGenericType()
        {
            Throws<Exception>(
                "Unrecognized discriminator for `Furniture` type.",
                () => JsonConvert.DeserializeObject<Chair>("{\"comfort\":0.8,\"cost\":360.0}"));
        }

        [TestMethod]
        public void Test_Deserialize_ArrayOfIAndSubtypes_With_Custom_Discriminator()
        {
            var objFurnitures = JsonConvert.DeserializeObject<Furniture[]>(
                "[{\"h\":1.2,\"cost\":1800.0,\"class\":\"Desk\"},{\"comfort\":0.8,\"cost\":360.0,\"class\":\"Chair\"}]");

            IsInstanceOfType(objFurnitures[0], typeof(Desk));
            var objDesk = objFurnitures[0] as Desk;
            Debug.Assert(objDesk != null, "objDesk != null");
            AreEqual(1.2f, objDesk.Height);
            AreEqual(1800f, objDesk.Cost);

            IsInstanceOfType(objFurnitures[1], typeof(Chair));
            var objChair = objFurnitures[1] as Chair;
            Debug.Assert(objChair != null, "objChair != null");
            AreEqual(0.8f, objChair.ComfortLevel);
            AreEqual(360f, objChair.Cost);
        }

        [TestMethod]
        public void Test_Deserialize_ArrayOfIAndSubtypes_ContainingNull()
        {
            var objFurnitures = JsonConvert.DeserializeObject<Furniture[]>("[null]");
            AreEqual(objFurnitures[0], null);
        }

        [TestMethod]
        public void Test_Deserialize_IAndSubtypes_Null()
        {
            var objFurniture = JsonConvert.DeserializeObject<Furniture>("null");
            AreEqual(objFurniture, null);
        }

        [TestMethod]
        public void Test_Serialize_SubclassLion_Null()
        {
            var objLion = JsonConvert.DeserializeObject<Lion>("null");
            AreEqual(objLion, null);
        }

        [TestMethod]
        public void Test_Serialize_SubclassFeline_Null()
        {
            var objFeline = JsonConvert.DeserializeObject<Feline>("null");
            AreEqual(objFeline, null);
        }

        [TestMethod]
        public void Test_Serialize_ClassAnimal_Null()
        {
            var objAnimal = JsonConvert.DeserializeObject<Animal>("null");
            AreEqual(objAnimal, null);
        }

        [TestMethod]
        public void Test_Serialize_Subclass_With_Discriminator_DefinedInMiddleClass()
        {
            var jsonTextBox = JsonConvert.SerializeObject(new TextBoxModel { Default = "A", Name = "N", Text = "T" });
            AreEqual(jsonTextBox, "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\"}");
        }

        [TestMethod]
        public void Test_Serialize_Subclass_With_Discriminator_DefinedNowhere()
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
        public void Test_Serialize_Subclass_With_Discriminator_DefinedInMiddleClass_ButNotSet()
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
        public void Test_Serialize_Subclass_Without_Discriminator_NorDefinedNorSet()
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
        public void Test_Serialize_Subclass_With_MultipleLevelDiscriminators()
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
        public void Test_Deserialize_Class_Without_Discriminator_DefinedInMiddleClass()
        {
            Throws<Exception>(
                "Cannot create object of type `FluentJsonNetTests.Models.Controls.ControlModel`",
                () => JsonConvert.DeserializeObject<ControlModel>(
                    "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\"}"));
        }

        [TestMethod]
        public void Test_Deserialize_Subclass_Without_Discriminator_DefinedInMiddleClass()
        {
            Throws<Exception>(
                "Cannot create object of type `FluentJsonNetTests.Models.Controls.EditorModel`",
                () => JsonConvert.DeserializeObject<EditorModel>(
                    "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\"}"));
        }

        [TestMethod]
        public void Test_Deserialize_SubclassFinal_With_Discriminator_SetButNotDefined()
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
        public void Test_Deserialize_SubclassFinal_Without_Discriminator_SetButNotDefined()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMap),
                    typeof(EditorModelMapNoDiscriminator),
                    typeof(TextBoxModelMap),
                });

            var textBox = JsonConvert.DeserializeObject<TextBoxModel>(
                "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\"}");
            AreEqual("T", textBox.Text);
            AreEqual("A", textBox.Default);
            AreEqual("N", textBox.Name);
        }

        [TestMethod]
        public void Test_Deserialize_SubclassMiddle_With_Discriminator_SetButNotDefined()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMap),
                    typeof(EditorModelMapNoDiscriminator),
                    typeof(TextBoxModelMap),
                });

            Throws<Exception>(
                "Cannot create object of type `FluentJsonNetTests.Models.Controls.EditorModel`",
                () => JsonConvert.DeserializeObject<EditorModel>(
                    "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\"}"));
        }

        [TestMethod]
        public void Test_Deserialize_SubclassMiddle_With_Discriminator_NotSetButDefined()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMap),
                    typeof(EditorModelMap),
                    typeof(TextBoxModelMapNoDiscriminator),
                });

            Throws<Exception>(
                "Value of discriminator field not verified by any subclass map.",
                () => JsonConvert.DeserializeObject<EditorModel>(
                    "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\"}"));
        }

        [TestMethod]
        public void Test_Deserialize_SubclassMiddle_Without_Discriminator_NotSetNorDefined()
        {
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(new[]
                {
                    typeof(ControlModelMap),
                    typeof(EditorModelMapNoDiscriminator),
                    typeof(TextBoxModelMapNoDiscriminator),
                });
            Throws<Exception>(
                "Cannot create object of type `FluentJsonNetTests.Models.Controls.EditorModel`",
                () => JsonConvert.DeserializeObject<EditorModel>(
                    "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\"}"));
        }

        [TestMethod]
        public void Test_Deserialize_SubclassMiddle_With_Discriminator_MiddleDefinedAndSet()
        {
            var control = JsonConvert.DeserializeObject<EditorModel>(
                "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\",\"editor\":\"TextBox\"}");

            IsInstanceOfType(control, typeof(TextBoxModel));
            var textBox = (TextBoxModel)control;
            AreEqual("T", textBox.Text);
            AreEqual("A", textBox.Default);
            AreEqual("N", textBox.Name);
        }

        [TestMethod]
        public void Test_Deserialize_Class_With_MultipleLevelDiscriminators()
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
        public void Test_Deserialize_SubclassMiddle_With_MultipleLevelDiscriminators()
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
        public void Test_Deserialize_SubclassFinal_With_MultipleLevelDiscriminators()
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
        public void Test_Deserialize_SubclassMiddle_With_MultipleLevelDiscriminatorsDefined_ButSerializedFinal()
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
        public void Test_Deserialize_SubclassMiddle_With_MultipleLevelDiscriminatorsDefined_ButSerializedFinalWrong()
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
        public void Test_Deserialize_SubclassMiddle_With_MultipleLevelDiscriminatorsDefined_ButSerializedNone()
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
        public void Test_Deserialize_Class_With_MultipleLevelDiscriminatorsDefined_SerializedMultiple()
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
        public void Test_Deserialize_Class_With_MultipleLevelDiscriminatorsDefined_SerializedFinal()
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
        public void Test_Deserialize_Class_With_MultipleLevelDiscriminatorsDefined_SerializedNone()
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
        public void Test_Deserialize_SubclassFinal_Without_Discriminator_MiddleDefinedAndSet()
        {
            var control = JsonConvert.DeserializeObject<TextBoxModel>(
                "{\"text\":\"T\",\"default\":\"A\",\"name\":\"N\"}");

            IsInstanceOfType(control, typeof(TextBoxModel));
            var textBox = (TextBoxModel)control;
            AreEqual("T", textBox.Text);
            AreEqual("A", textBox.Default);
            AreEqual("N", textBox.Name);
        }
    }
}