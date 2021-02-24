using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default
{
    public class DefaultJsonToken : IJsonToken
    {
        public JToken Token { get; }

        private protected readonly JsonSerializer _serializer;

        public DefaultJsonToken(JToken token, JsonSerializer serializer)
        {
            Token = token;
            _serializer = serializer;
        }

        public T ToType<T>()
            => Token.ToObject<T>(_serializer);

        public string ToIndentedString()
            => Token.ToString(Formatting.Indented);

        public override string ToString()
            => Token.ToString(Formatting.None);

        [return: NotNullIfNotNull("token")]
        public static IJsonToken? Create(JToken? token, JsonSerializer serializer)
        {
            if (token == null)
                return null;

            if (token is JObject @object)
                return new DefaultJsonObject(@object, serializer);

            if (token is JArray array)
                return new DefaultJsonArray(array, serializer);

            if (token is JProperty property)
                return new DefaultJsonProperty(property, serializer);

            if (token is JValue value)
                return new DefaultJsonValue(value, serializer);

            throw new InvalidOperationException("Unknown JSON token type.");
        }
    }
}
