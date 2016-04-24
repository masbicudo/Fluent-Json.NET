using System;

namespace FluentJsonNet
{
    public abstract class JsonSubclassMap<T> : JsonSubclassMap
    {
        public override Type SerializedType { get; } = typeof(T);
    }

    public abstract class JsonSubclassMap : JsonMapBase
    {
        internal string DiscriminatorFieldValue;

        protected void DiscriminatorValue(string fieldValue)
        {
            this.DiscriminatorFieldValue = fieldValue;
        }

        public virtual object CreateNew()
        {
            return Activator.CreateInstance(this.SerializedType);
        }
    }
}