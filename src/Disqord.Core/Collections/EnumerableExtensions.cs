using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Disqord.Collections
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class EnumerableExtensions
    {
        public static T[] GetArray<T>(this IEnumerable<T> enumerable)
            => enumerable is T[] array
                ? array
                : enumerable.ToArray();
    }
}
