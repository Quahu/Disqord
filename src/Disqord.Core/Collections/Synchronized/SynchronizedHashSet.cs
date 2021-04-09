using System.Collections;
using System.Collections.Generic;

namespace Disqord.Collections.Synchronized
{
    /// <summary>
    ///     A lightweight (<see langword="lock"/>-based) thread-safe implementation of <see cref="ISet{T}"/>.
    /// </summary>
    public sealed class SynchronizedHashSet<T> : ISet<T>, IReadOnlySet<T>
    {
        public int Count
        {
            get
            {
                lock (this)
                {
                    return _hashSet.Count;
                }
            }
        }

        bool ICollection<T>.IsReadOnly => false;

        private readonly HashSet<T> _hashSet;

        public SynchronizedHashSet() : this(0)
        { }

        public SynchronizedHashSet(int capacity)
        {
            _hashSet = new HashSet<T>(capacity);
        }

        public SynchronizedHashSet(IEnumerable<T> collection)
        {
            _hashSet = new HashSet<T>(collection);
        }

        public bool Add(T item)
        {
            lock (this)
            {
                return _hashSet.Add(item);
            }
        }

        public void Clear()
        {
            lock (this)
            {
                _hashSet.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (this)
            {
                return _hashSet.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (this)
            {
                _hashSet.CopyTo(array, arrayIndex);
            }
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            lock (this)
            {
                _hashSet.ExceptWith(other);
            }
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            lock (this)
            {
                _hashSet.IntersectWith(other);
            }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            lock (this)
            {
                return _hashSet.IsProperSubsetOf(other);
            }
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            lock (this)
            {
                return _hashSet.IsProperSupersetOf(other);
            }
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            lock (this)
            {
                return _hashSet.IsSubsetOf(other);
            }
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            lock (this)
            {
                return _hashSet.IsSupersetOf(other);
            }
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            lock (this)
            {
                return _hashSet.Overlaps(other);
            }
        }

        public bool Remove(T item)
        {
            lock (this)
            {
                return _hashSet.Remove(item);
            }
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            lock (this)
            {
                return _hashSet.SetEquals(other);
            }
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            lock (this)
            {
                _hashSet.SymmetricExceptWith(other);
            }
        }

        public void UnionWith(IEnumerable<T> other)
        {
            lock (this)
            {
                _hashSet.UnionWith(other);
            }
        }

        public T[] ToArray()
        {
            lock (this)
            {
                var array = new T[_hashSet.Count];
                _hashSet.CopyTo(array);
                return array;
            }
        }

        public IEnumerator<T> GetEnumerator()
            => (ToArray() as IReadOnlyList<T>).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        void ICollection<T>.Add(T item)
            => Add(item);
    }
}
