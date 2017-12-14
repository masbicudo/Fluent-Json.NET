using System;
using System.Reflection;

namespace FluentJsonNet
{
    public abstract class JsonSubclassMap<T> : JsonSubclassMap
    {
        public override Type SerializedType { get; } = typeof(T);
    }

    public abstract class JsonSubclassMap : JsonMapBase
    {
        internal string DiscriminatorFieldValue;

        /// <summary>
        /// Defines the value of the discriminator field that matches the currently mapped type.
        /// </summary>
        /// <param name="fieldValue"></param>
        protected void DiscriminatorValue(string fieldValue)
        {
            this.DiscriminatorFieldValue = fieldValue;
        }

        public override object CreateNew()
        {
            return Activator.CreateInstance(this.SerializedType);
        }

        public override bool CanCreateNew()
        {
            var typeInfo = this.SerializedType.GetTypeInfo();
            return !typeInfo.IsAbstract
                   && !typeInfo.IsInterface
                   && (typeInfo.IsValueType || typeInfo.GetConstructor(new Type[0]) != null);
        }
    }
}