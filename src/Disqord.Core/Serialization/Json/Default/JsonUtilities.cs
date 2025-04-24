using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Disqord.Serialization.Json.Default;

internal static class JsonUtilities
{
    public const double MaxSafeInteger = 9007199254740991;

    public static ulong ReadUInt64FromString(this ref Utf8JsonReader reader)
    {
        if (!reader.HasValueSequence)
        {
            return ulong.Parse(reader.ValueSpan);
        }

        if (reader.ValueSequence.IsSingleSegment)
        {
            return ulong.Parse(reader.ValueSequence.FirstSpan);
        }

        var buffer = (stackalloc byte[20]);
        reader.ValueSequence.CopyTo(buffer);
        return ulong.Parse(buffer[..(int) reader.ValueSequence.Length]);
    }

    [StackTraceHidden]
    [DoesNotReturn]
    public static void RethrowJsonException(JsonException ex)
    {
        throw new JsonException(ex.Message, new JsonException(null, ex.InnerException));
    }
}
