using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace FluentJsonNet.Compat
{
    static class TypeExtraInfo
    {
        public static bool MembersAreTheSame(MemberInfo a, MemberInfo b)
        {
#if NETSTANDARD1_0
            return a.Name == b.Name
                   && a.DeclaringType == b.DeclaringType
                   && a.GetMemberType() == b.GetMemberType()
                   && PropertiesEqual(a as PropertyInfo, b as PropertyInfo);

            bool PropertiesEqual(PropertyInfo pa, PropertyInfo pb) =>
                pa != null && pb != null
                && pa.PropertyType == pb.PropertyType && pa.CanWrite == pb.CanWrite;

            bool FieldsEqual(FieldInfo fa, FieldInfo fb) =>
                fa != null && fb != null
                && fa.IsInitOnly == fb.IsInitOnly
                && fa.IsLiteral == fb.IsLiteral
                && fa.IsStatic == fb.IsStatic
                && fa.IsPublic == fb.IsPublic
                && fa.FieldType == fb.FieldType;
#else
            return a.MetadataToken == b.MetadataToken
                   && a.DeclaringType == b.DeclaringType;
#endif
        }
    }
}
