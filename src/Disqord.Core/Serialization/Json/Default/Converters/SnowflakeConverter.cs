using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Disqord.Serialization.Json.Default;

internal sealed class SnowflakeConverter : JsonConverter<Snowflake>
{
    /// <inheritdoc />
    public override bool HandleNull => false;

    public override Snowflake Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.ReadUInt64FromString();
        }

        return reader.GetUInt64();
    }

    public override Snowflake ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.ReadUInt64FromString();
    }

    public override void Write(Utf8JsonWriter writer, Snowflake value, JsonSerializerOptions options)
    {
        var buffer = (stackalloc byte[20]);
        value.RawValue.TryFormat(buffer, out var countWritten);
        writer.WriteStringValue(buffer[..countWritten]);
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, Snowflake value, JsonSerializerOptions options)
    {
        var buffer = (stackalloc byte[20]);
        value.RawValue.TryFormat(buffer, out var countWritten);
        writer.WritePropertyName(buffer[..countWritten]);
    }
}
