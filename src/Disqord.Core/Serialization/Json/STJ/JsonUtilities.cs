using System.Text.Json;
#if NET8_0_OR_GREATER
using System.Buffers;
#endif

namespace Disqord.Serialization.Json.System;

internal static class JsonUtilities
{
    public static ulong ReadUInt64FromString(this ref Utf8JsonReader reader)
    {
#if NET8_0_OR_GREATER
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

#else
        var buffer = (stackalloc char[20]);
        reader.CopyString(buffer);
        return ulong.Parse(buffer);
#endif
    }
}
