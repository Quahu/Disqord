using System.Collections.Generic;

namespace Disqord.Collections.Synchronized
{
    public class SynchronizedList<T> : ISynchronizedList<T>
    {
        public int Count
        {
            get
            {
                lock (this)
                {
                    return _list.Count;
                }
            }
        }

        public T this[int index]
        {
            get
            {
                lock (this)
                {
                    return _list[index];
                }
            }
            set
            {
                lock (this)
                {
                    _list[index] = value;
                }
            }
        }

        bool ICollection<T>.IsReadOnly => false;

        private readonly IList<T> _list;

        public SynchronizedList()
        {
            _list = new List<T>();
        }

        public SynchronizedList(int capacity)
        {
            _list = new List<T>(capacity);
        }

        public SynchronizedList(IList<T> list)
        {
            _list = list;
        }

        public void Add(T item)
        {
            lock (this)
            {
                _list.Add(item);
            }
        }

        public void Clear()
        {
            lock (this)
            {
                _list.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (this)
            {
                return _list.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (this)
            {
                _list.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(T item)
        {
            lock (this)
            {
                return _list.Remove(item);
            }
        }

        public int IndexOf(T item)
        {
            lock (this)
            {
                return _list.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (this)
            {
                _list.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (this)
            {
                _list.RemoveAt(index);
            }
        }
    }
}
