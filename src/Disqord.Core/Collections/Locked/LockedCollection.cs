//using System.Collections;
//using System.Collections.Generic;

//namespace Disqord.Collections
//{
//    internal sealed class LockedCollection<T> : ICollection<T>, IReadOnlyCollection<T>
//    {
//        public int Count
//        {
//            get
//            {
//                lock (_lock)
//                {
//                    return _collection.Count;
//                }
//            }
//        }

//        public bool IsReadOnly => _collection.IsReadOnly;

//        private readonly ICollection<T> _collection;
//        private readonly object _lock;

//        public LockedCollection(ICollection<T> collection, object @lock)
//        {
//            _collection = collection;
//            _lock = @lock;
//        }

//        public void Add(T item)
//        {
//            lock (_lock)
//            {
//                _collection.Add(item);
//            }
//        }

//        public void Clear()
//        {
//            lock (_lock)
//            {
//                _collection.Clear();
//            }
//        }

//        public bool Contains(T item)
//        {
//            lock (_lock)
//            {
//                return _collection.Contains(item);
//            }
//        }

//        public void CopyTo(T[] array, int arrayIndex)
//        {
//            lock (_lock)
//            {
//                _collection.CopyTo(array, arrayIndex);
//            }
//        }

//        public bool Remove(T item)
//        {
//            lock (_lock)
//            {
//                return _collection.Remove(item);
//            }
//        }

//        public IEnumerator<T> GetEnumerator()
//            => _collection.GetEnumerator().Locked(_lock);

//        IEnumerator IEnumerable.GetEnumerator()
//            => GetEnumerator();
//    }
//}
