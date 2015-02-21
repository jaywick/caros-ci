using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Publisher
{
    public static class Extensions
    {
        public static bool Matches(this string message, string pattern)
        {
            return new Regex(pattern).IsMatch(message);
        }

        public static IEnumerable<string> Extract(this string message, string pattern)
        {
            foreach (Capture item in new Regex(pattern).Match(message).Captures)
                yield return item.Value;
        }

        public static string Substitute(this string message, string pattern, string replacement)
        {
            return Regex.Replace(message, pattern, replacement);
        }
    }
}
