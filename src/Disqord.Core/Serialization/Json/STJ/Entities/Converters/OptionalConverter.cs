using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Qommon;
using Qommon.Serialization;

namespace Disqord.Serialization.Json.System;

internal sealed class OptionalConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableTo(typeof(IOptional))
            && typeToConvert.IsValueType
            && typeToConvert.IsConstructedGenericType
            && typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return Activator.CreateInstance(typeof(OptionalConverterImpl<>).MakeGenericType(typeToConvert.GenericTypeArguments[0])) as JsonConverter;
    }

    private sealed class OptionalConverterImpl<TValue> : JsonConverter<Optional<TValue?>>
    {
        public override Optional<TValue?> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<TValue>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, Optional<TValue?> value, JsonSerializerOptions options)
        {
            var optionalValue = value.Value;
            if (optionalValue == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                JsonSerializer.Serialize(writer, value.Value, options);
            }
        }
    }
}
