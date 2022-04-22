using System;
using Newtonsoft.Json;
using Qommon.Serialization;

namespace Disqord.Serialization.Json.Default
{
    internal sealed class OptionalConverter : JsonConverter
    {
        public static readonly OptionalConverter Instance = new OptionalConverter(null);

        private readonly JsonConverter _converter;

        private OptionalConverter(JsonConverter converter)
        {
            _converter = converter;
        }

        public override bool CanConvert(Type objectType)
            => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => objectType.GetConstructors()[0].Invoke(new[] { serializer.Deserialize(reader, objectType.GenericTypeArguments[0]) });

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var optionalValue = ((IOptional) value).Value;
            if (optionalValue == null)
            {
                writer.WriteNull();
            }
            else
            {
                if (_converter != null)
                {
                    _converter.WriteJson(writer, optionalValue, serializer);
                }
                else
                {
                    serializer.Serialize(writer, optionalValue);
                }
            }
        }

        public static OptionalConverter Create(JsonConverter converter = null)
            => converter != null
                ? new OptionalConverter(converter)
                : Instance;
    }
}
