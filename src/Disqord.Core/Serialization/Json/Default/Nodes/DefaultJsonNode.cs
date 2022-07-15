using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default
{
    /// <summary>
    ///     Represents a default JSON node.
    ///     Wraps a <see cref="JToken"/>.
    /// </summary>
    public class DefaultJsonNode : IJsonNode
    {
        /// <summary>
        ///     Gets the underlying <see cref="JToken"/>.
        /// </summary>
        public JToken Token { get; }

        private protected readonly JsonSerializer Serializer;

        public DefaultJsonNode(JToken token, JsonSerializer serializer)
        {
            Token = token;
            Serializer = serializer;
        }

        /// <inheritdoc/>
        public T ToType<T>()
            => Token.ToObject<T>(Serializer);

        /// <summary>
        ///     Formats this node into a JSON representation with the specified formatting.
        /// </summary>
        /// <param name="formatting"> The formatting to use. </param>
        /// <returns>
        ///     The string representing this node.
        /// </returns>
        public string ToString(Formatting formatting)
            => Token.ToString(formatting);

        /// <summary>
        ///     Formats this node into an indented JSON representation.
        /// </summary>
        /// <returns>
        ///     The string representing this node.
        /// </returns>
        public override string ToString()
            => Token.ToString(Formatting.Indented);

        /// <summary>
        ///     Creates a new <see cref="DefaultJsonNode"/> from the specified object.
        /// </summary>
        /// <param name="obj"> The object to create the node for. </param>
        /// <param name="serializer"> The default JSON serializer. </param>
        /// <returns>
        ///     A JSON node representing the object.
        /// </returns>
        public static IJsonNode Create(object obj, DefaultJsonSerializer serializer)
        {
            var token = JToken.FromObject(obj);
            return Create(token, serializer.UnderlyingSerializer);
        }

        internal static IJsonNode Create(JToken token, JsonSerializer serializer)
            => token switch
            {
                null => null,
                JObject @object => new DefaultJsonObject(@object, serializer),
                JArray array => new DefaultJsonArray(array, serializer),
                JValue value => new DefaultJsonValue(value, serializer),
                _ => throw new InvalidOperationException("Unknown JSON token type.")
            };
    }
}
