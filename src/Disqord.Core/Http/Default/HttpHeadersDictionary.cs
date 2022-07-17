using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;

namespace Disqord.Http.Default;

/// <summary>
///     This class is a bit pointless, but I'm trying to make life at least a tiny bit easier by wrapping some of the dictionary methods
///     from <see cref="HttpHeaders"/> in an actual dictionary class, as <see cref="HttpHeaders"/> for whatever reason doesn't implement anything besides <see cref="IEnumerable{T}"/>.
///     I might rework this into an expression based accessor for <see cref="HttpHeaders"/>' underlying private dictionary.
/// </summary>
internal sealed class HttpHeadersDictionary : IDictionary<string, string>, IReadOnlyDictionary<string, string>
{
    public ICollection<string> Keys => this.Select(x => x.Key).ToArray();

    public ICollection<string> Values => this.Select(x => x.Value).ToArray();

    public int Count => _headers.Count();

    public bool IsReadOnly => true;

    IEnumerable<string> IReadOnlyDictionary<string, string>.Keys => this.Select(x => x.Key);

    IEnumerable<string> IReadOnlyDictionary<string, string>.Values => this.Select(x => x.Value);

    public string this[string key]
    {
        get => _headers.TryGetValues(key, out var values)
            ? values.First()
            : throw new KeyNotFoundException();
        set => throw new NotSupportedException();
    }

    private readonly HttpHeaders _headers;

    public HttpHeadersDictionary(HttpHeaders headers)
    {
        _headers = headers;
    }

    public bool Contains(KeyValuePair<string, string> item)
    {
        return _headers.Contains(item.Key);
    }

    public bool ContainsKey(string key)
    {
        return _headers.Contains(key);
    }

    public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
    {
        foreach (var (headerName, headerValue) in _headers.NonValidated)
        {
            array[arrayIndex++] = KeyValuePair.Create(headerName, headerValue.ToString());
        }
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
    {
        if (_headers.TryGetValues(key, out var values))
        {
            value = values.First();
            return true;
        }

        value = null;
        return false;
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        foreach (var kvp in _headers)
            yield return KeyValuePair.Create(kvp.Key, kvp.Value.First());
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(string key, string value)
        => throw new NotSupportedException();

    public void Add(KeyValuePair<string, string> item)
        => throw new NotSupportedException();

    public void Clear()
        => throw new NotSupportedException();

    public bool Remove(string key)
        => throw new NotSupportedException();

    public bool Remove(KeyValuePair<string, string> item)
        => throw new NotSupportedException();
}
