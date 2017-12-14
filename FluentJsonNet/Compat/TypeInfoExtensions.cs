using System.Collections.Generic;
using System.Linq;

namespace System.Reflection
{
    static class TypeInfoExtensions
    {
#if NETSTANDARD1_0
        public static ConstructorInfo GetConstructor(this TypeInfo ti, Type[] types)
        {
            return ti.DeclaredConstructors.Single(
                c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(types));
        }
#elif NET45
        public static ConstructorInfo GetConstructor(this TypeInfo ti, Type[] types)
        {
            return ti.AsType().GetConstructor(types);
        }
#endif
    }
}
