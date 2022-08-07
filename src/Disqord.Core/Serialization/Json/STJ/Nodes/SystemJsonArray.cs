using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Disqord.Serialization.Json.System
{
    internal class SystemJsonArray : SystemJsonNode, IJsonArray
    {
        public new JsonArray Token => base.Token.AsArray();

        public SystemJsonArray(JsonNode node, JsonSerializerOptions options)
            : base(node, options)
        { }

        public IEnumerator<IJsonNode?> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => Token.Count;

        public IJsonNode? this[int index] => Create(Token[index], Options);

        private sealed class Enumerator : IEnumerator<IJsonNode?>
        {
            public IJsonNode? Current => Create(_current?.Token, _array.Options);

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
                    _current = (_array[index] as SystemJsonArray)!;
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
}
