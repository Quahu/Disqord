using System;
using System.IO;

namespace Disqord.Serialization.Json
{
    /// <summary>
    ///     Represents errors that occur in an <see cref="IJsonSerializer"/>.
    /// </summary>
    public class JsonSerializationException : Exception
    {
        /// <summary>
        ///     Gets whether it was the <see cref="IJsonSerializer.Deserialize{T}(Stream)"/> or <see cref="IJsonSerializer.Serialize(object)"/> that failed.
        /// </summary>
        public bool IsDeserialize { get; }

        /// <summary>
        ///     Instantiates a new <see cref="JsonSerializationException"/>.
        /// </summary>
        public JsonSerializationException(bool isDeserialize, Type type, Exception exception)
            : base($"An exception occurred while attempting to {(isDeserialize ? "deserialize" : "serialize")} JSON {(isDeserialize ? "into" : "from")} {type}.", exception)
        {
            IsDeserialize = isDeserialize;
        }
    }
}
