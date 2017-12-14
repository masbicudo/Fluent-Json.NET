#if NETSTANDARD1_0
namespace System.Reflection
{
    static class ReflectionExtensions
    {
        public static MethodInfo GetGetMethod(this PropertyInfo pi, bool nonPublic)
        {
            if (nonPublic == true)
                throw new InvalidOperationException("netstandard1.0 does not support getting private property");
            return pi.GetMethod;
        }

        public static MethodInfo GetSetMethod(this PropertyInfo pi, bool nonPublic)
        {
            if (nonPublic == true)
                throw new InvalidOperationException("netstandard1.0 does not support getting private property");
            return pi.SetMethod;
        }

        public static MethodInfo GetGetMethod(this PropertyInfo pi)
        {
            return pi.GetMethod;
        }

        public static MethodInfo GetSetMethod(this PropertyInfo pi)
        {
            return pi.SetMethod;
        }

        public static InterfaceMapping GetInterfaceMap(this TypeInfo typeinfo, Type interfaceType)
        {
            return typeinfo.GetRuntimeInterfaceMap(interfaceType);
        }

        public static MemberTypes GetMemberType(this MemberInfo mi)
        {
            return (mi is PropertyInfo ? MemberTypes.Property : 0)
                | (mi is MethodInfo ? MemberTypes.Method : 0)
                | (mi is EventInfo ? MemberTypes.Event : 0)
                | (mi is ConstructorInfo ? MemberTypes.Constructor : 0)
                | (mi is FieldInfo ? MemberTypes.Field : 0)
                | (mi is TypeInfo ? (mi.DeclaringType != null ? MemberTypes.NestedType : MemberTypes.TypeInfo) : 0);
        }

        public static MethodInfo GetBaseDefinition(this MethodInfo mi)
        {
            return mi.GetRuntimeBaseDefinition();
        }
    }

    public enum MemberTypes
    {
        Constructor = 1,
        Event = 2,
        Field = 4,
        Method = 8,
        Property = 16,
        TypeInfo = 32,
        Custom = 64,
        NestedType = 128,
        All = 191
    }
}
#endif