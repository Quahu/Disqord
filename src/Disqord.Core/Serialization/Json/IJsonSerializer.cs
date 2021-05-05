using System;
using System.IO;
using Disqord.Logging;

namespace Disqord.Serialization.Json
{
    /// <summary>
    ///     Represents a JSON (de)serializer.
    /// </summary>
    public interface IJsonSerializer : ILogging
    {
        /// <summary>
        ///     Deserializes the specified UTF-8 JSON to the specified type.
        /// </summary>
        /// <param name="json"> The JSON to deserialize. </param>
        /// <typeparam name="T"> The type to deserialize to. </typeparam>
        /// <returns>
        ///     The deserialized instance.
        /// </returns>
        T Deserialize<T>(Stream json)
            where T : class;

        /// <summary>
        ///     Serializes the specified object to UTF-8 JSON.
        /// </summary>
        /// <param name="obj"> The object to serialize. </param>
        /// <returns>
        ///     The serialized object.
        /// </returns>
        Memory<byte> Serialize(object obj);

        /// <summary>
        ///     Gets a JSON node from the specified argument.
        /// </summary>
        /// <param name="obj"> The object to get the JSON node for. </param>
        /// <returns>
        ///     A JSON node representing the object.
        /// </returns>
        IJsonNode GetJsonNode(object obj);
    }
}
