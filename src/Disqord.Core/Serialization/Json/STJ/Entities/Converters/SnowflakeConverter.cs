using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using BufferType =
#if NET8_0_OR_GREATER
    byte
#else
    char
#endif
    ;

namespace Disqord.Serialization.Json.System;

internal class SnowflakeConverter : JsonConverter<Snowflake>
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

    public override void Write(Utf8JsonWriter writer, Snowflake value, JsonSerializerOptions options)
    {
        var buffer = (stackalloc BufferType[20]);
        value.RawValue.TryFormat(buffer, out var countWritten);
        writer.WriteStringValue(buffer[..countWritten]);
    }
}
