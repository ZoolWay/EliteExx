using System;

namespace Zw.EliteExx.Core
{
    public static class CharExtensions
    {
        public static bool IsDefaultOrWhitespace(this char c)
        {
            if (c == default(char)) return true;
            return Char.IsWhiteSpace(c);
        }
    }
}
