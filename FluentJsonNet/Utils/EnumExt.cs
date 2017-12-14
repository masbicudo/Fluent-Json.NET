using System;
using System.Collections.Generic;

namespace FluentJsonNet.Utils
{
    internal static class EnumExt
    {
        public static IEnumerable<T> WithMax<T>(this IEnumerable<T> enumerable, Func<T, int?> valueGetter)
        {
            int? max = null;
            var maxItem = new List<T>();
            foreach (var item in enumerable)
            {
                var val = valueGetter(item);
                if (max == null)
                {
                    max = val;
                    maxItem.Clear();
                    maxItem.Add(item);
                }
                else if (max < val)
                {
                    max = val;
                    maxItem.Clear();
                    maxItem.Add(item);
                }
                else if (max == val)
                {
                    maxItem.Add(item);
                }
            }

            return maxItem;
        }
    }
}
