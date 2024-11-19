using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Disqord.Serialization.Json.System;

/// <summary>
///     Represents a default JSON object node.
///     Wraps a <see cref="JsonObject"/>.
/// </summary>
internal sealed class SystemJsonObject : SystemJsonNode, IJsonObject
{
    /// <inheritdoc cref="SystemJsonNode.Node"/>
    public new JsonObject Node => (base.Node as JsonObject)!;

    /// <inheritdoc/>
    public int Count => Node.Count;

    /// <inheritdoc/>
    public ICollection<string> Keys => (Node as IDictionary<string, JsonNode?>).Keys;

    /// <inheritdoc/>
    public ICollection<IJsonNode?> Values => (Node as IDictionary<string, JsonNode?>).Values.Select(value => Create(value, Options)).ToArray();

    /// <inheritdoc/>
    public IJsonNode? this[string key]
    {
        get => Create(Node[key], Options);
        set => Node[key] = GetSystemNode(value);
    }

    bool ICollection<KeyValuePair<string, IJsonNode?>>.IsReadOnly => false;

    internal SystemJsonObject(JsonObject @object, JsonSerializerOptions options)
        : base(@object, options)
    { }

    /// <inheritdoc/>
    public void Add(KeyValuePair<string, IJsonNode?> item)
    {
        Add(item.Key, item.Value);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        Node.Clear();
    }

    /// <inheritdoc/>
    public bool Contains(KeyValuePair<string, IJsonNode?> item)
    {
        return TryGetValue(item.Key, out var value) && ReferenceEquals(value, item.Value);
    }

    /// <inheritdoc/>
    public void CopyTo(KeyValuePair<string, IJsonNode?>[] array, int arrayIndex)
    {
        var index = 0;
        foreach (var (key, value) in this)
        {
            array[arrayIndex + index++] = KeyValuePair.Create(key, value);
        }
    }

    /// <inheritdoc/>
    public bool Remove(KeyValuePair<string, IJsonNode?> item)
    {
        return Remove(item.Key);
    }

    /// <inheritdoc/>
    public void Add(string key, IJsonNode? value)
    {
        Node.Add(key, GetSystemNode(value));
    }

    /// <inheritdoc/>
    public bool ContainsKey(string key)
    {
        return Node.ContainsKey(key);
    }

    /// <inheritdoc/>
    public bool TryGetValue(string key, out IJsonNode? value)
    {
        if (Node.TryGetPropertyValue(key, out var node))
        {
            value = Create(node, Options);
            return true;
        }

        value = null;
        return false;
    }

    /// <inheritdoc/>
    public bool Remove(string key)
    {
        return Node.Remove(key);
    }

    private sealed class Enumerator : IEnumerator<KeyValuePair<string, IJsonNode?>>
    {
        public KeyValuePair<string, IJsonNode?> Current => KeyValuePair.Create(_enumerator.Current.Key, Create(_enumerator.Current.Value, _options));

        object IEnumerator.Current => Current;

        private readonly IEnumerator<KeyValuePair<string, JsonNode?>> _enumerator;
        private readonly JsonSerializerOptions _options;

        internal Enumerator(IEnumerator<KeyValuePair<string, JsonNode?>> enumerator, JsonSerializerOptions options)
        {
            _enumerator = enumerator;
            _options = options;
        }

        public bool MoveNext()
            => _enumerator.MoveNext();

        public void Reset()
            => _enumerator.Reset();

        public void Dispose()
            => _enumerator.Dispose();
    }

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<string, IJsonNode?>> GetEnumerator()
    {
        return new Enumerator(Node.GetEnumerator(), Options);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
