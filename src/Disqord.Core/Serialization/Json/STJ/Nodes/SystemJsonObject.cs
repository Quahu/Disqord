using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Disqord.Serialization.Json.System;

public class SystemJsonObject : SystemJsonNode, IJsonObject
{
    private readonly JsonSerializerOptions _options;

    public new JsonObject Token => base.Token.AsObject();

    public SystemJsonObject(JsonObject node, JsonSerializerOptions options)
        : base(node, options)
    {
        _options = options;
    }

    public int Count => Token.Count;

    public bool ContainsKey(string key)
    {
        return Token.ContainsKey(key);
    }

    public bool TryGetValue(string key, out IJsonNode? value)
    {
        if (Token.TryGetPropertyValue(key, out var propertyValue))
        {
            value = Create(propertyValue, _options);
            return true;
        }

        value = null;
        return false;
    }

    public IJsonNode? this[string key] => Create(Token[key], _options);

    public IEnumerable<string> Keys => (Token as IDictionary<string, JsonNode?>).Keys;

    public IEnumerable<IJsonNode?> Values => (Token as IDictionary<string, JsonNode?>).Values.Select(x => Create(x, _options));

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

    public IEnumerator<KeyValuePair<string, IJsonNode?>> GetEnumerator()
    {
        return new Enumerator(Token.GetEnumerator(), _options);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
