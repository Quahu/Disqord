using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Qommon;

namespace Disqord.Serialization.Json.Default;

internal sealed class EnumConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeToConvert.GetCustomAttribute<StringEnumAttribute>() != null
            ? typeof(StringEnumConverterImpl<>)
            : typeof(NumberEnumConverterImpl<>);

        return Activator.CreateInstance(converterType.MakeGenericType(typeToConvert)) as JsonConverter;
    }

    private class NumberEnumConverterImpl<TEnum> : JsonConverter<TEnum>
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

        public override TEnum ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.ReadUInt64FromString();
            return Unsafe.As<ulong, TEnum>(ref value);
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            var ulongValue = ((IConvertible) value).ToUInt64(CultureInfo.InvariantCulture);
            if (ulongValue <= JsonUtilities.MaxSafeInteger)
            {
                writer.WriteNumberValue(ulongValue);
            }
            else
            {
                var buffer = (stackalloc byte[20]);
                ulongValue.TryFormat(buffer, out var countWritten);
                writer.WriteStringValue(buffer[..countWritten]);
            }
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            var buffer = (stackalloc byte[20]);
            var ulongValue = ((IConvertible) value).ToUInt64(CultureInfo.InvariantCulture);
            ulongValue.TryFormat(buffer, out var countWritten);
            writer.WritePropertyName(buffer[..countWritten]);
        }
    }

    private class StringEnumConverterImpl<TEnum> : JsonConverter<TEnum>
        where TEnum : struct, Enum
    {
        private readonly JsonEncodedText[] _names;
        private readonly TEnum[] _values;

        public StringEnumConverterImpl()
        {
            var names = Enum.GetNames<TEnum>();
            var fields = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<EnumMemberAttribute>() is not EnumMemberAttribute enumMemberAttribute
                    || enumMemberAttribute.Value == null)
                {
                    continue;
                }

                for (var i = 0; i < names.Length; i++)
                {
                    if (field.Name != names[i])
                    {
                        continue;
                    }

                    names[i] = enumMemberAttribute.Value;
                    break;
                }
            }

            _names = Array.ConvertAll(names, static name => JsonEncodedText.Encode(name));
            _values = Enum.GetValues<TEnum>();
        }

        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if ((!reader.HasValueSequence
                    ? reader.ValueSpan
                    : reader.ValueSequence.FirstSpan)[0] is >= 0x30 and <= 0x39)
                {
                    // The string value is a number
                    var numberValue = reader.ReadUInt64FromString();
                    return Unsafe.As<ulong, TEnum>(ref numberValue);
                }

                for (var i = 0; i < _names.Length; i++)
                {
                    var name = _names[i];

                    // TODO: performance check
                    if (reader.ValueTextEquals(name.EncodedUtf8Bytes))
                    {
                        return _values[i];
                    }
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                var numberValue = reader.GetUInt64();
                return Unsafe.As<ulong, TEnum>(ref numberValue);
            }

            Throw.InvalidOperationException($"Invalid enum value {reader.TokenType}: '{Encoding.UTF8.GetString(reader.ValueSpan)}'.");
            return default;
        }

        public override TEnum ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TEnum value = default;
            for (var i = 0; i < _names.Length; i++)
            {
                var name = _names[i];

                // TODO: performance check
                if (reader.ValueTextEquals(name.EncodedUtf8Bytes))
                {
                    value = _values[i];
                    break;
                }
            }

            return value;
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            var index = Array.IndexOf(_values, value);
            if (index != -1)
            {
                writer.WriteStringValue(_names[index]);
            }
            else
            {
                Throw.InvalidOperationException("Invalid enum value.");
            }
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            var index = Array.IndexOf(_values, value);
            if (index != -1)
            {
                writer.WritePropertyName(_names[index]);
            }
            else
            {
                Throw.InvalidOperationException("Invalid enum value.");
            }
        }
    }
}
