using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default
{
    public sealed class DefaultJsonElement : IJsonElement
    {
        public bool IsArray => Token is JArray;

        public JToken Token { get; }

        private readonly JsonSerializer _serializer;

        public DefaultJsonElement(JToken token, JsonSerializer serializer)
        {
            Token = token;
            _serializer = serializer;
        }

        public T ToType<T>()
            => Token.ToObject<T>(_serializer);

        public override string ToString()
            => Token.ToString(Formatting.Indented);
    }
}
