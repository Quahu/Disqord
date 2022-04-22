using System.Collections.Generic;
using Qommon;
using Qommon.Collections;

namespace Disqord
{
    internal static class LocalConstructUtilities
    {
        public static bool Add<T>(this Optional<IList<T>> optional, T item, out IList<T> list)
        {
            if (optional.TryGetValue(out list) && list != null && !list.IsReadOnly)
            {
                list.Add(item);
                return true;
            }

            list = list != null
                ? new List<T>(list)
                : new List<T>();

            list.Add(item);
            return false;
        }

        public static bool With<T>(this Optional<IList<T>> optional, IEnumerable<T> items, out IList<T> list)
        {
            if (optional.TryGetValue(out list) && list != null && !list.IsReadOnly)
            {
                list.Clear();
                list.AddRange(items);
                return true;
            }

            if (list != null)
            {
                list = new List<T>(list);
                list.AddRange(items);
            }
            else
            {
                list = new List<T>(items);
            }

            return false;
        }
    }
}
