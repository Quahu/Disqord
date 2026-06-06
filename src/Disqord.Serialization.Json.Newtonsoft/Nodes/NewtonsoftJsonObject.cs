using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Newtonsoft;

/// <summary>
///     Represents a default JSON object node.
///     Wraps a <see cref="JObject"/>.
/// </summary>
public class NewtonsoftJsonObject : NewtonsoftJsonNode, IJsonObject
{
    /// <inheritdoc cref="NewtonsoftJsonNode.Token"/>
    public new JObject Token => (base.Token as JObject)!;

    /// <inheritdoc/>
    public int Count => Token.Count;

    /// <inheritdoc/>
    public ICollection<string> Keys => (Token as IDictionary<string, JToken>).Keys;

    /// <inheritdoc/>
    public ICollection<IJsonNode?> Values => (Token as IDictionary<string, JToken>).Values.Select(value => Create(value, Serializer)).ToArray();

    /// <inheritdoc/>
    public IJsonNode? this[string key]
    {
        get => Create(Token[key], Serializer);
        set => Token[key] = GetJToken(value);
    }

    bool ICollection<KeyValuePair<string, IJsonNode?>>.IsReadOnly => false;

    public NewtonsoftJsonObject(JObject token, JsonSerializer serializer)
        : base(token, serializer)
    { }

    /// <inheritdoc/>
    public void Add(KeyValuePair<string, IJsonNode?> item)
    {
        Add(item.Key, item.Value);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        Token.RemoveAll();
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
        Token.Add(key, GetJToken(value));
    }

    /// <inheritdoc/>
    public bool ContainsKey(string key)
    {
        return Token.ContainsKey(key);
    }

    /// <inheritdoc/>
    public bool TryGetValue(string key, out IJsonNode? value)
    {
        if (Token.TryGetValue(key, out var token))
        {
            value = Create(token, Serializer);
            return true;
        }

        value = null;
        return false;
    }

    /// <inheritdoc/>
    public bool Remove(string key)
    {
        return Token.Remove(key);
    }

    private sealed class Enumerator : IEnumerator<KeyValuePair<string, IJsonNode?>>
    {
        public KeyValuePair<string, IJsonNode?> Current => KeyValuePair.Create(_enumerator.Current.Key, Create(_enumerator.Current.Value, _serializer));

        object IEnumerator.Current => Current;

        private readonly IEnumerator<KeyValuePair<string, JToken?>> _enumerator;
        private readonly JsonSerializer _serializer;

        internal Enumerator(IEnumerator<KeyValuePair<string, JToken?>> enumerator, JsonSerializer serializer)
        {
            _enumerator = enumerator;
            _serializer = serializer;
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
        return new Enumerator(Token.GetEnumerator(), Serializer);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
