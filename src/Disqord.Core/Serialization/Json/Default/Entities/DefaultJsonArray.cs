using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default
{
    public class DefaultJsonArray : DefaultJsonToken, IJsonArray
    {
        public new JArray Token => base.Token as JArray;

        public int Count => Token.Count;

        public IJsonToken this[int index] => Create(Token[index], _serializer);

        public DefaultJsonArray(JArray token, JsonSerializer serializer)
            : base(token, serializer)
        { }

        private sealed class Enumerator : IEnumerator<IJsonToken>
        {
            public IJsonToken Current => Create(_current.Token, _array._serializer);
            object IEnumerator.Current => Current;

            private readonly DefaultJsonArray _array;
            private int _index;
            private DefaultJsonToken? _current;

            public Enumerator(DefaultJsonArray array)
            {
                _array = array;
            }

            public bool MoveNext()
            {
                var index = _index;
                if (_index++ < _array.Count)
                {
                    _current = _array[index] as DefaultJsonToken;
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

        public IEnumerator<IJsonToken> GetEnumerator()
            => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
