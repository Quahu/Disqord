using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default;

/// <summary>
///     Represents a default JSON object node.
///     Wraps a <see cref="JObject"/>.
/// </summary>
public class DefaultJsonObject : DefaultJsonNode, IJsonObject
{
    /// <inheritdoc cref="DefaultJsonNode.Token"/>
    public new JObject Token => (base.Token as JObject)!;

    /// <inheritdoc/>
    public int Count => Token.Count;

    /// <inheritdoc/>
    public IEnumerable<string> Keys => (Token as IDictionary<string, JToken>).Keys;

    /// <inheritdoc/>
    public IEnumerable<IJsonNode?> Values => (Token as IDictionary<string, JToken>).Values.Select(x => Create(x, Serializer));

    /// <inheritdoc/>
    public IJsonNode? this[string key] => Create(Token[key], Serializer);

    public DefaultJsonObject(JObject token, JsonSerializer serializer)
        : base(token, serializer)
    { }

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
