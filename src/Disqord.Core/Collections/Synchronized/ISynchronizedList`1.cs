using System.Collections;
using System.Collections.Generic;

namespace Disqord.Collections.Synchronized
{
    public interface ISynchronizedList<T> : IList<T>
    {
        T[] ToArray()
        {
            lock (this)
            {
                var array = new T[Count];
                CopyTo(array, 0);
                return array;
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => (ToArray() as IList<T>).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
