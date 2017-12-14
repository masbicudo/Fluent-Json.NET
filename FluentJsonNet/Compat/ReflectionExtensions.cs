#if !NETSTANDARD1_0
namespace System.Reflection
{
    static class ReflectionExtensions
    {
        public static MemberTypes GetMemberType(this MemberInfo mi)
        {
            return mi.MemberType;
        }
    }
}
#endif