using Disqord.Serialization.Json.Default;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Disqord.Serialization.Json.STJ.Nodes
{
    internal class STJJsonArray : STJJsonNode, IJsonArray
    {
        private readonly JsonSerializerOptions _options;
        public new JsonArray Token => base.Token.AsArray();
        public STJJsonArray(JsonNode node, JsonSerializerOptions options) : base(node)
        {
            _options = options;
        }

        public IEnumerator<IJsonNode?> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => Token.Count;

        public IJsonNode? this[int index] => Create(Token[index], _options);

        private sealed class Enumerator : IEnumerator<IJsonNode?>
        {
            public IJsonNode? Current => Create(_current?.Token, _array._options);

            object? IEnumerator.Current => Current;

            private readonly STJJsonArray _array;
            private int _index;
            private STJJsonNode? _current;

            internal Enumerator(STJJsonArray array)
            {
                _array = array;
            }

            public bool MoveNext()
            {
                var index = _index;
                if (_index++ < _array.Count)
                {
                    _current = (_array[index] as STJJsonArray)!;
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
