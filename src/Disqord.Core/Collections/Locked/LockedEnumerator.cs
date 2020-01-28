using System.Collections.Generic;

namespace Disqord.Collections
{
    internal static class LockedEnumerator
    {
        public static LockedEnumerator<T> Create<T>(IEnumerator<T> enumerator, object @lock)
            => new LockedEnumerator<T>(enumerator, @lock);
    }
}
