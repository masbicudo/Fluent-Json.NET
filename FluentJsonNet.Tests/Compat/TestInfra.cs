using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

#if NET40 || NET45
namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TestClassAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestInitializeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestMethodAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExpectedException : Attribute
    {
        Type exceptionType;

        public ExpectedException(Type exceptionType)
        {
            this.exceptionType = exceptionType;
        }

        public bool Accept(Exception ex) => ex.GetType() == this.exceptionType;
    }

    public class Assert
    {
        public static Asserter Default;

        public static void AreEqual<T>(T expected, T value)
        {
            Default.AreEqual(expected, value);
        }

        public static void IsInstanceOfType(object value, Type expectedType, string message = null)
        {
            Default.IsInstanceOfType(value, expectedType, message);
        }

        public static void Inconclusive(string message, params object[] parameters)
        {
            Default.Inconclusive(message, parameters);
        }

        public static void IsTrue(bool what)
        {
            Default.IsTrue(what);
        }
    }

    public abstract class Asserter
    {
        public string CurrentClass { get; set; }
        public string CurrentMethod { get; set; }
        public TestState State { get; set; }

        public abstract void AreEqual<T>(T expected, T value);

        public abstract void IsInstanceOfType(object value, Type expectedType, string message);

        public abstract void Inconclusive(string message, object[] parameters);

        public abstract void IsTrue(bool what);
    }

    public enum TestState
    {
        Ok = 0,
        Inconclusive = 1,
        Error = 3,
    }
}

namespace System.Reflection
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<T> GetCustomAttributes<T>(this MethodInfo mi)
        {
            return mi.GetCustomAttributes(typeof(T), true).OfType<T>();
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this Type t)
        {
            return t.GetTypeInfo().GetCustomAttributes(typeof(T), true).OfType<T>();
        }
    }
}
#endif