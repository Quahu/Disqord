using System;

namespace Disqord.Serialization.Json
{
    /// <summary>
    ///     Represents errors that occur in an <see cref="IJsonSerializer"/>.
    /// </summary>
    public class JsonSerializationException : Exception
    {
        /// <summary>
        ///     Gets whether it was the <see cref="IJsonSerializer.Deserialize{T}(ReadOnlyMemory{byte})"/> or <see cref="IJsonSerializer.Serialize(object)"/> that failed.
        /// </summary>
        public bool IsDeserialize { get; }

        /// <summary>
        ///     Gets the UTF-8 JSON that the deserialization failed for. Not present when <see cref="IsDeserialize"/> is <see langword="false"/>.
        /// </summary>
        public ReadOnlyMemory<byte> Json { get; }

        /// <summary>
        ///     Instantiates a new <see cref="JsonSerializationException"/>.
        /// </summary>
        public JsonSerializationException(bool isDeserialize, ReadOnlyMemory<byte> json, Type type, Exception exception)
            : base($"An exception occurred while attempting to {(isDeserialize ? "deserialize" : "serialize")} JSON {(isDeserialize ? "into" : "from")} the type {type.Name}.", exception)
        {
            IsDeserialize = isDeserialize;
            Json = json;
        }
    }
}
