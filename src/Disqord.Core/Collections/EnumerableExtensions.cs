using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Disqord.Collections
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class EnumerableExtensions
    {
        public static T[] GetArray<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return Array.Empty<T>();

            if (enumerable is T[] array)
                return array;

            return enumerable.ToArray();
        }
    }
}
