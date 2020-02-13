//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading;

//namespace Disqord.Collections
//{
//    internal sealed class LockedEnumerator<T> : IEnumerator<T>
//    {
//        public T Current => _enumerator.Current;

//        object IEnumerator.Current => _enumerator.Current;

//        private readonly IEnumerator<T> _enumerator;
//        private readonly object _lock;

//        public LockedEnumerator(IEnumerator<T> enumerator, object @lock)
//        {
//            _enumerator = enumerator;
//            _lock = @lock;
//            if (!Monitor.TryEnter(@lock, 10000))
//                throw new InvalidOperationException("The collection lock has been taken for longer than 10 seconds.");
//        }

//        public bool MoveNext()
//            => _enumerator.MoveNext();

//        public void Reset()
//            => _enumerator.Reset();

//        public void Dispose()
//        {
//            _enumerator.Dispose();
//            Monitor.Exit(_lock);
//        }
//    }
//}
