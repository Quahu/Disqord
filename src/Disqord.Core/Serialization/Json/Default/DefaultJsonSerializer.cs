using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default
{
    /// <summary>
    ///     Represents the default <see cref="IJsonSerializer"/>.
    ///     Uses Newtonsoft.Json.
    /// </summary>
    public sealed class DefaultJsonSerializer : IJsonSerializer
    {
        /// <inheritdoc/>
        public ILogger Logger { get; }

        /// <summary>
        ///     Gets the underlying <see cref="JsonSerializer"/>.
        /// </summary>
        public JsonSerializer UnderlyingSerializer { get; }

        internal readonly bool ShowHttpStreamsWarning;

        private static readonly Encoding Utf8 = new UTF8Encoding(false);

        public DefaultJsonSerializer(
            IOptions<DefaultJsonSerializerConfiguration> options,
            ILogger<DefaultJsonSerializer> logger)
        {
            ShowHttpStreamsWarning = options.Value.ShowHttpStreamsWarning;
            Logger = logger;

            UnderlyingSerializer = new JsonSerializer
            {
                ContractResolver = new ContractResolver(this)
            };
        }

        /// <inheritdoc/>
        public T Deserialize<T>(Stream json)
            where T : class
        {
            try
            {
                using (var textReader = new StreamReader(json, Utf8, leaveOpen: true))
                using (var jsonReader = new JsonTextReader(textReader))
                {
                    return UnderlyingSerializer.Deserialize<T>(jsonReader);
                }
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException(true, typeof(T), ex);
            }
        }

        /// <inheritdoc/>
        public Memory<byte> Serialize(object model)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream, Utf8, leaveOpen: true))
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    UnderlyingSerializer.Serialize(jsonWriter, JToken.FromObject(model, UnderlyingSerializer));

                    jsonWriter.Flush();
                    memoryStream.TryGetBuffer(out var buffer);
                    return buffer;
                }
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException(false, model.GetType(), ex);
            }
        }

        /// <inheritdoc/>
        public IJsonNode GetJsonNode(object value)
        {
            if (value == null)
                return DefaultJsonNode.Create(JValue.CreateNull(), UnderlyingSerializer);

            return DefaultJsonNode.Create(JToken.FromObject(value, UnderlyingSerializer), UnderlyingSerializer);
        }
    }
}
