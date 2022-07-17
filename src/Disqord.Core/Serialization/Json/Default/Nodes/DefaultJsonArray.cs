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
    public IJsonNode? this[int index] => Create(Token[index], Serializer);

    public DefaultJsonArray(JArray token, JsonSerializer serializer)
        : base(token, serializer)
    { }

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