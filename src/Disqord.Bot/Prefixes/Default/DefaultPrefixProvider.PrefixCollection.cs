using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord.Bot.Prefixes
{
    public sealed partial class DefaultPrefixProvider
    {
        public sealed class PrefixCollection : ICollection<IPrefix>, IReadOnlyCollection<IPrefix>
        {
            public int Count
            {
                get
                {
                    lock (_prefixes)
                    {
                        return _prefixes.Count;
                    }
                }
            }

            private readonly HashSet<IPrefix> _prefixes;

            bool ICollection<IPrefix>.IsReadOnly => false;

            internal PrefixCollection()
            {
                _prefixes = new HashSet<IPrefix>();
            }

            public void Add(IPrefix prefix)
            {
                if (prefix == null)
                    throw new ArgumentNullException(nameof(prefix));

                lock (_prefixes)
                {
                    _prefixes.Add(prefix);
                }
            }

            public void Clear()
            {
                lock (_prefixes)
                {
                    _prefixes.Clear();
                }
            }

            public bool Contains(IPrefix item)
            {
                lock (_prefixes)
                {
                    return _prefixes.Contains(item);
                }
            }

            public void CopyTo(IPrefix[] array, int arrayIndex)
            {
                lock (_prefixes)
                {
                    _prefixes.CopyTo(array, arrayIndex);
                }
            }

            public bool Remove(IPrefix item)
            {
                lock (_prefixes)
                {
                    return _prefixes.Remove(item);
                }
            }

            public IPrefix[] ToArray()
            {
                lock (_prefixes)
                {
                    var array = new IPrefix[_prefixes.Count];
                    _prefixes.CopyTo(array);
                    return array;
                }
            }

            public IEnumerator<IPrefix> GetEnumerator()
                => (ToArray() as IReadOnlyList<IPrefix>).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }
    }
}
