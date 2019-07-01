using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Zw.EliteExx.Core
{
    public static class EnumerableExtensions
    {
        public static ImmutableArray<T> ToImmutableArrayOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) return ImmutableArray<T>.Empty;
            return enumerable.ToImmutableArray<T>();
        }
    }
}
