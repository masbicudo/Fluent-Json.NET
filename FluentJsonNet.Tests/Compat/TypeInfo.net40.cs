using System.Collections.Generic;
using System.Linq;

#if NET40
namespace System.Reflection
{
    class TypeInfo
    {
        private readonly Type type;

        public TypeInfo(Type type)
        {
            this.type = type;
        }

        public bool IsGenericType => this.type.IsGenericType;

        public bool IsEnum => this.type.IsEnum;

        public bool ContainsGenericParameters => this.type.ContainsGenericParameters;

        public bool IsAbstract => this.type.IsAbstract;

        public Type BaseType => this.type.BaseType;

        public bool IsInterface => this.type.IsInterface;

        public bool IsValueType => this.type.IsValueType;

        public Assembly Assembly => this.type.Assembly;

        public bool IsAssignableFrom(TypeInfo typeInfo)
        {
            return this.type.IsAssignableFrom(typeInfo.type);
        }

        public IEnumerable<Attribute> GetCustomAttributes(Type type, bool inherit)
        {
            return this.type.GetCustomAttributes(type, inherit).OfType<Attribute>();
        }

        public InterfaceMapping GetInterfaceMap(Type interfaceType)
        {
            return this.type.GetInterfaceMap(interfaceType);
        }

        public PropertyInfo GetDeclaredProperty(string name)
        {
            return this.type.GetProperty(name,
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly);
        }

        public bool IsDefined(Type type, bool inherit)
        {
            return this.type.IsDefined(type, inherit);
        }

        public ConstructorInfo GetConstructor(Type[] types)
        {
            return this.type.GetConstructor(types);
        }

        public Type AsType() => this.type;
    }
}
#endif