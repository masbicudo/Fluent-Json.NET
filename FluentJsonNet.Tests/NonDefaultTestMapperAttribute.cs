using System;

namespace FluentJsonNet.Tests
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class NonDefaultTestMapperAttribute : Attribute
    {
        public NonDefaultTestMapperAttribute()
        {
        }
    }
}