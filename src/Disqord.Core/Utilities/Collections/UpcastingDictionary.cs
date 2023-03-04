using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Qommon;

namespace Disqord;

internal sealed class UpcastingDictionary<TKey, TOriginalValue, TNewValue> : IDictionary<TKey, TNewValue>, IReadOnlyDictionary<TKey, TNewValue>
    where TKey : notnull
    where TOriginalValue : class, TNewValue
{
    public IEnumerable<TKey> Keys => _dictionary.Keys;

    public IEnumerable<TNewValue> Values => _dictionary.Values;

    ICollection<TKey> IDictionary<TKey, TNewValue>.Keys => Keys.ToArray();

    ICollection<TNewValue> IDictionary<TKey, TNewValue>.Values => Values.ToArray();

    public int Count => _dictionary.Count;

    public bool IsReadOnly => _dictionary.IsReadOnly;

    private readonly IDictionary<TKey, TOriginalValue> _dictionary;

    public UpcastingDictionary(IReadOnlyDictionary<TKey, TOriginalValue> dictionary)
    {
        _dictionary = Guard.IsAssignableToType<IDictionary<TKey, TOriginalValue>>(dictionary);
    }

    public UpcastingDictionary(IDictionary<TKey, TOriginalValue> dictionary)
    {
        _dictionary = dictionary;
    }

    public TNewValue this[TKey key]
    {
        get => _dictionary[key];
        set => _dictionary[key] = (TOriginalValue) value!;
    }

    public void Add(TKey key, TNewValue value)
    {
        _dictionary.Add(key, (TOriginalValue) value!);
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        return _dictionary.Remove(key);
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TNewValue value)
    {
        if (_dictionary.TryGetValue(key, out var oldValue))
        {
            value = oldValue;
            return true;
        }

        value = default;
        return false;
    }

    public void Add(KeyValuePair<TKey, TNewValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TNewValue> item)
    {
        return ContainsKey(item.Key);
    }

    public void CopyTo(KeyValuePair<TKey, TNewValue>[] array, int arrayIndex)
    {
        foreach (var (key, value) in _dictionary)
        {
            array[arrayIndex++] = new KeyValuePair<TKey, TNewValue>(key, value);
        }
    }

    public bool Remove(KeyValuePair<TKey, TNewValue> item)
    {
        return _dictionary.Remove(item.Key);
    }

    public IEnumerator<KeyValuePair<TKey, TNewValue>> GetEnumerator()
    {
        foreach (var kvp in _dictionary)
        {
            yield return new KeyValuePair<TKey, TNewValue>(kvp.Key, kvp.Value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
