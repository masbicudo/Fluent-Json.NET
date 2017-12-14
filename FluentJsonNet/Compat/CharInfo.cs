using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FluentJsonNet.Compat
{
    static class CharInfo
    {
        public static UnicodeCategory GetUnicodeCategory(this char ch)
        {
#if NETSTANDARD1_0
            return CharUnicodeInfo.GetUnicodeCategory(ch);
#else
            return char.GetUnicodeCategory(ch);
#endif
        }
    }
}
