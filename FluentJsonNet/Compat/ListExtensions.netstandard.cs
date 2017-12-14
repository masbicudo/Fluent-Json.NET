using System.Collections.Generic;
using System.Collections.ObjectModel;

#if NETSTANDARD1_0
namespace System.Reflection
{
    static class ListExtensions
    {
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list)
        {
            return new ReadOnlyCollection<T>(list);
        }
    }
}
#endif