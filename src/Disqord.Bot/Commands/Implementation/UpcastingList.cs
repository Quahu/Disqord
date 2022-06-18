using System.Collections.Generic;
using Qommon.Collections.Proxied;

namespace Disqord.Bot.Commands;

internal class UpcastingList<T, TUp> : ProxiedList<T>, IList<TUp>, IReadOnlyList<TUp>
    where T : class, TUp
    where TUp : class
{
    TUp IList<TUp>.this[int index]
    {
        get => this[index];
        set => this[index] = (T) value;
    }

    TUp IReadOnlyList<TUp>.this[int index] => this[index];

    void ICollection<TUp>.Add(TUp item)
    {
        Add((T) item);
    }

    bool ICollection<TUp>.Contains(TUp item)
    {
        return Contains((T) item);
    }

    void ICollection<TUp>.CopyTo(TUp[] array, int arrayIndex)
    {
        var list = List;
        var count = list.Count;
        for (var i = 0; i < count; i++)
            array[arrayIndex + i] = list[i];
    }

    bool ICollection<TUp>.Remove(TUp item)
    {
        return Remove((T) item);
    }

    int IList<TUp>.IndexOf(TUp item)
    {
        return IndexOf((T) item);
    }

    void IList<TUp>.Insert(int index, TUp item)
    {
        Insert(index, (T) item);
    }

    IEnumerator<TUp> IEnumerable<TUp>.GetEnumerator()
    {
        var list = List;
        var count = list.Count;
        for (var i = 0; i < count; i++)
            yield return list[i];
    }
}
