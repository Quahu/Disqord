using System;
using System.ComponentModel;
using System.IO;

namespace Disqord.Serialization.Json;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class JsonSerializerExtensions
{
    /// <summary>
    ///     Deserializes the UTF-8 JSON stream into an object.
    /// </summary>
    /// <param name="serializer"> The JSON serializer. </param>
    /// <param name="stream"> The JSON stream to deserialize from. </param>
    /// <typeparam name="T"> The type to deserialize to. </typeparam>
    /// <returns>
    ///     The deserialized instance or <see langword="null"/>.
    /// </returns>
    public static T? Deserialize<T>(this IJsonSerializer serializer, Stream stream)
    {
        return (T?) serializer.Deserialize(stream, typeof(T));
    }

    /// <summary>
    ///     Serializes the specified object to UTF-8 JSON.
    /// </summary>
    /// <param name="serializer"> The JSON serializer. </param>
    /// <param name="obj"> The object to serialize. </param>
    /// <param name="options"> The serializer options. </param>
    /// <returns>
    ///     The serialized object.
    /// </returns>
    public static Memory<byte> Serialize(this IJsonSerializer serializer, object obj, IJsonSerializerOptions? options = null)
    {
        using (var memoryStream = new MemoryStream())
        {
            serializer.Serialize(memoryStream, obj, options);
            memoryStream.TryGetBuffer(out var buffer);
            return buffer;
        }
    }
}