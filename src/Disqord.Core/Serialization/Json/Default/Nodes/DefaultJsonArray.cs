using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default;

/// <summary>
///     Represents a default JSON array node.
///     Wraps a <see cref="JArray"/>.
/// </summary>
public class DefaultJsonArray : DefaultJsonNode, IJsonArray
{
    /// <inheritdoc cref="DefaultJsonNode.Token"/>
    public new JArray Token => (base.Token as JArray)!;

    /// <inheritdoc/>
    public int Count => Token.Count;

    /// <inheritdoc/>
    public IJsonNode? this[int index]
    {
        get => Create(Token[index], Serializer);
        set => Token[index] = GetJToken(value)!;
    }

    bool ICollection<IJsonNode?>.IsReadOnly => false;

    public DefaultJsonArray(JArray token, JsonSerializer serializer)
        : base(token, serializer)
    { }

    /// <inheritdoc/>
    public void Add(IJsonNode? item)
    {
        Token.Add(GetJToken(item)!);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        Token.Clear();
    }

    /// <inheritdoc/>
    public bool Contains(IJsonNode? item)
    {
        return Token.Contains(GetJToken(item)!);
    }

    /// <inheritdoc/>
    public void CopyTo(IJsonNode?[] array, int arrayIndex)
    {
        var count = Count;
        for (var i = 0; i < count; i++)
        {
            array[arrayIndex + i] = this[i];
        }
    }

    /// <inheritdoc/>
    public bool Remove(IJsonNode? item)
    {
        return Token.Remove(GetJToken(item)!);
    }

    /// <inheritdoc/>
    public int IndexOf(IJsonNode? item)
    {
        return Token.IndexOf(GetJToken(item)!);
    }

    /// <inheritdoc/>
    public void Insert(int index, IJsonNode? item)
    {
        Token.Insert(index, GetJToken(item)!);
    }

    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        Token.RemoveAt(index);
    }

    /// <inheritdoc/>
    public IEnumerator<IJsonNode?> GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private sealed class Enumerator : IEnumerator<IJsonNode?>
    {
        public IJsonNode? Current => Create(_current?.Token, _array.Serializer);

        object? IEnumerator.Current => Current;

        private readonly DefaultJsonArray _array;
        private int _index;
        private DefaultJsonNode? _current;

        internal Enumerator(DefaultJsonArray array)
        {
            _array = array;
        }

        public bool MoveNext()
        {
            var index = _index;
            if (_index++ < _array.Count)
            {
                _current = (_array[index] as DefaultJsonNode)!;
                return true;
            }

            _current = null;
            return false;
        }

        public void Reset()
        {
            _index = 0;
            _current = null;
        }

        public void Dispose()
        { }
    }
}
