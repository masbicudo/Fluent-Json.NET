using System;

namespace FluentJsonNet
{
    public interface IAndSubtypes
    {
        Type SerializedType { get; }
        Func<Type, string> DiscriminatorFieldValueGetter { get; }
        object CreateObject(string discriminatorValue);
    }

    public interface IAndSubtypes<out T> :
        IAndSubtypes
    {
        new T CreateObject(string discriminatorValue);
    }
}