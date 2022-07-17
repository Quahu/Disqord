using System;
using System.IO;
using Disqord.Logging;

namespace Disqord.Serialization.Json;

/// <summary>
///     Represents a JSON (de)serializer.
/// </summary>
public interface IJsonSerializer : ILogging
{
    /// <summary>
    ///     Deserializes the UTF-8 JSON stream into an object.
    /// </summary>
    /// <param name="stream"> The JSON stream to deserialize from. </param>
    /// <param name="type"> The type to deserialize to. </param>
    /// <returns>
    ///     The deserialized instance or <see langword="null"/>.
    /// </returns>
    object? Deserialize(Stream stream, Type type);

    /// <summary>
    ///     Serializes the object into the stream.
    /// </summary>
    /// <param name="stream"> The stream to serialize to. </param>
    /// <param name="obj"> The object to serialize. </param>
    /// <param name="options"> The serializer options. </param>
    void Serialize(Stream stream, object obj, IJsonSerializerOptions? options = null);

    /// <summary>
    ///     Gets a JSON node from the specified argument.
    /// </summary>
    /// <param name="obj"> The object to get the JSON node for. </param>
    /// <returns>
    ///     A JSON node representing the object.
    /// </returns>
    IJsonNode GetJsonNode(object? obj);
}