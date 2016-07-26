using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FluentJsonNet.Utils
{
    public static class NamingConventions
    {
        public static string CamelCase(string name)
        {
            return string.Join(
                "",
                SeparateNames(name).Select(
                    (n, i) =>
                        i == 0
                            ? n.ToLowerInvariant()
                            : n.Length == 0
                                ? ""
                                : FirstToUpperInvariant(n)));
        }

        public static string UpperCamelCase(string name)
        {
            return string.Join(
                "",
                SeparateNames(name).Select(
                    (n, i) =>
                        n.Length == 0
                            ? ""
                            : FirstToUpperInvariant(n)));
        }

        public static string AllUpperUnderscoreSeparated(string name)
        {
            return string.Join(
                "_",
                SeparateNames(name).Select(
                    (n, i) => n.ToUpperInvariant()));
        }

        public static string AllLowerHyphenSeparated(string name)
        {
            return string.Join(
                "-",
                SeparateNames(name).Select(
                    (n, i) => n.ToLowerInvariant()));
        }

        public static string FirstToUpperInvariant(string str)
        {
            return str.Length == 0 ? "" : char.ToUpper(str[0]) + str.Substring(1).ToLowerInvariant();
        }

        public static IEnumerable<string> SeparateNames(string name)
        {
            var ucaseSequence = 0;
            var prevStart = 0;
            for (int it = 0; it < name.Length; it++)
            {
                var ch = name[it];
                if (char.IsUpper(ch))
                {
                    if (it - prevStart != ucaseSequence)
                    {
                        yield return name.Substring(prevStart, it - prevStart);
                        prevStart = it;
                    }

                    ucaseSequence++;
                }
                else if (ch == '_' || ch == '-' || char.GetUnicodeCategory(ch) == UnicodeCategory.SpaceSeparator)
                {
                    if (it - prevStart != 0)
                        yield return name.Substring(prevStart, it - prevStart);

                    ucaseSequence = 0;
                    prevStart = it + 1;
                }
                else
                {
                    if (ucaseSequence > 0)
                    {
                        if (ucaseSequence - 1 != 0)
                        {
                            yield return name.Substring(prevStart, ucaseSequence - 1);
                            prevStart += ucaseSequence - 1;
                        }

                        ucaseSequence = 0;
                    }
                }
            }

            if (name.Length - prevStart != 0)
                yield return name.Substring(prevStart, name.Length - prevStart);
        }
    }
}