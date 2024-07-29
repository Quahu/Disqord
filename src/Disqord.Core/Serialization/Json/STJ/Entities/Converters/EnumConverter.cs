using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Qommon;
using BufferType =
#if NET8_0_OR_GREATER
    byte
#else
    char
#endif
    ;
namespace Disqord.Serialization.Json.System;

public class EnumConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return Activator.CreateInstance(typeof(EnumConverterImpl<>).MakeGenericType(typeToConvert)) as JsonConverter;
    }

    private class EnumConverterImpl<TEnum> : JsonConverter<TEnum>
        where TEnum : struct, Enum
    {
        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = 0ul;
            if (reader.TokenType == JsonTokenType.String)
            {
                value = reader.ReadUInt64FromString();
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                value = reader.GetUInt64();
            }
            else
            {
                Throw.InvalidOperationException("Invalid enum value.");
            }

            return Unsafe.As<ulong, TEnum>(ref value);
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            const double maxSafeInteger = 9007199254740991;

            var ulongValue = ((IConvertible) value).ToUInt64(CultureInfo.InvariantCulture);
            if (ulongValue <= maxSafeInteger)
            {
                writer.WriteNumberValue(ulongValue);
            }
            else
            {
                var buffer = (stackalloc BufferType[20]);
                ulongValue.TryFormat(buffer, out var countWritten);
                writer.WriteStringValue(buffer[..countWritten]);
            }
        }
    }
}
