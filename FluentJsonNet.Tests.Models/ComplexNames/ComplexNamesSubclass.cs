namespace FluentJsonNet.Tests
{
    public class ComplexNamesSubclass : ComplexNamesBase
    {
        public ComplexNamesSubclass(string nameOfTheEntity, string baseName)
            : base(baseName)
        {
            this.NameInSubclass = nameOfTheEntity;
        }

        public string NameInSubclass { get; set; }
    }
}