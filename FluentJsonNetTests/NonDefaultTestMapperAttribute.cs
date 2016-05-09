using System;

namespace FluentJsonNetTests
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class NonDefaultTestMapperAttribute : Attribute
    {
        public NonDefaultTestMapperAttribute()
        {
        }
    }
}