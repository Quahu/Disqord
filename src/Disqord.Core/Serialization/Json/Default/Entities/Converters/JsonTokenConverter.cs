using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default
{
    internal sealed class JsonElementConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.ReadFrom(reader);
            return new DefaultJsonElement(token, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var token = ((DefaultJsonElement) value).Token;
            serializer.Serialize(writer, token);
        }
    }
}
