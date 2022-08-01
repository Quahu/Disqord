using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Disqord.Serialization.Json.System;

internal class SnowflakeConverter : JsonConverter<Snowflake>
{
    /// <inheritdoc />
    public override bool HandleNull => false;

    public override Snowflake Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString()!;
        return new Snowflake(Snowflake.Parse(value));
    }

    public override void Write(Utf8JsonWriter writer, Snowflake value, JsonSerializerOptions options)
    {
        var stringValue = (stackalloc char[20]);
        value.TryFormat(stringValue, out var charsWritten);
        writer.WriteStringValue(stringValue[..charsWritten]);
    }
}
