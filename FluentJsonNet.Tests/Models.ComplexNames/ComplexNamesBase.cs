namespace FluentJsonNet.Tests
{
    public class ComplexNamesBase
    {
        public ComplexNamesBase(string nameOfTheEntity)
        {
            this.NameInBase = nameOfTheEntity;
        }

        public string NameInBase { get; set; }
    }
}
