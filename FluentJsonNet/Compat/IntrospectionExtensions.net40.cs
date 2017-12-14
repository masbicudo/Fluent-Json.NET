#if NET40
namespace System.Reflection
{
    static class IntrospectionExtensions
    {
        public static TypeInfo GetTypeInfo(this Type type)
        {
            return new TypeInfo(type);
        }
    }
}
#endif