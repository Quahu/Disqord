using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Disqord.Serialization.Json.Default;

internal sealed class StringConverter : JsonConverter<string?>
{
    /// <inheritdoc/>
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetUInt64().ToString();
        }

        if (reader.TokenType == JsonTokenType.True)
        {
            return bool.TrueString;
        }

        if (reader.TokenType == JsonTokenType.False)
        {
            return bool.FalseString;
        }

        return null;
    }

    /// <inheritdoc/>
    public override string? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString();
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.AsSpan());
        }
    }

    /// <inheritdoc/>
    public override void WriteAsPropertyName(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WritePropertyName(value.AsSpan());
    }
}
