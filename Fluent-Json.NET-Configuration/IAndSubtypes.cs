using System;

namespace FluentJsonNet
{
    public interface IAndSubtypes
    {
        Func<Type, string> DiscriminatorFieldValueGetter { get; }
        object CreateObject(string discriminatorValue);
    }
}