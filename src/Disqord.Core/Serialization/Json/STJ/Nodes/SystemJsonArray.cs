using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Disqord.Serialization.Json.System;

/// <summary>
///     Represents a default JSON array node.
///     Wraps a <see cref="JsonArray"/>.
/// </summary>
internal sealed class SystemJsonArray : SystemJsonNode, IJsonArray
{
    /// <inheritdoc cref="Token"/>
    public new JsonArray Node => (base.Node as JsonArray)!;

    /// <inheritdoc/>
    public int Count => Node.Count;

    /// <inheritdoc/>
    public IJsonNode? this[int index]
    {
        get => Create(Node[index], Options);
        set => Node[index] = GetSystemNode(value);
    }

    bool ICollection<IJsonNode?>.IsReadOnly => false;

    internal SystemJsonArray(JsonArray node, JsonSerializerOptions options)
        : base(node, options)
    { }

    /// <inheritdoc/>
    public void Add(IJsonNode? item)
    {
        Node.Add(GetSystemNode(item));
    }

    /// <inheritdoc/>
    public void Clear()
    {
        Node.Clear();
    }

    /// <inheritdoc/>
    public bool Contains(IJsonNode? item)
    {
        return Node.Contains(GetSystemNode(item));
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
        return Node.Remove(GetSystemNode(item));
    }

    /// <inheritdoc/>
    public int IndexOf(IJsonNode? item)
    {
        return Node.IndexOf(GetSystemNode(item));
    }

    /// <inheritdoc/>
    public void Insert(int index, IJsonNode? item)
    {
        Node.Insert(index, GetSystemNode(item));
    }

    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        Node.RemoveAt(index);
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
        public IJsonNode? Current => Create(_current?.Node, _array.Options);

        object? IEnumerator.Current => Current;

        private readonly SystemJsonArray _array;
        private int _index;
        private SystemJsonNode? _current;

        internal Enumerator(SystemJsonArray array)
        {
            _array = array;
        }

        public bool MoveNext()
        {
            var index = _index;
            if (_index++ < _array.Count)
            {
                _current = (_array[index] as SystemJsonNode)!;
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
