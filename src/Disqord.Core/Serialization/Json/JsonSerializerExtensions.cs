using System;
using System.ComponentModel;
using System.IO;

namespace Disqord.Serialization.Json;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class JsonSerializerExtensions
{
    /// <summary>
    ///     Serializes the specified object to UTF-8 JSON.
    /// </summary>
    /// <param name="serializer"> The JSON serializer. </param>
    /// <param name="obj"> The object to serialize. </param>
    /// <param name="options"> The serializer options. </param>
    /// <returns>
    ///     The serialized object.
    /// </returns>
    public static Memory<byte> Serialize(this IJsonSerializer serializer, object obj, JsonSerializationOptions? options = null)
    {
        using (var memoryStream = new MemoryStream())
        {
            serializer.Serialize(memoryStream, obj, options);
            memoryStream.TryGetBuffer(out var buffer);
            return buffer;
        }
    }
}
