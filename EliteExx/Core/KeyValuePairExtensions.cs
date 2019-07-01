using System;
using System.Collections.Generic;

namespace Zw.EliteExx.Core
{
    /// <summary>
    /// Extension methods for KeyValuePair.
    /// </summary>
    public static class KeyValuePairExtensions
    {
        /// <summary>
        /// Tuple deconstructor.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="kvp"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }
    }
}
