using System;
using System.Collections.Generic;

namespace Disqord
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> enumerable, int size)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be positive.");

            return InternalBatch(enumerable, size);
        }

        private static IEnumerable<IEnumerable<T>> InternalBatch<T>(IEnumerable<T> enumerable, int size)
        {
            using (var enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return InternalSingleBatch(enumerator, size);
                }
            }
        }

        private static IEnumerable<T> InternalSingleBatch<T>(IEnumerator<T> enumerator, int size)
        {
            do
            {
                yield return enumerator.Current;
            }
            while (--size > 0 && enumerator.MoveNext());
        }
    }
}
