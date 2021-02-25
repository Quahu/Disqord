using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default
{
    public sealed class DefaultJsonSerializer : IJsonSerializer
    {
        public ILogger Logger { get; }

        public JsonSerializer UnderlyingSerializer { get; }

        internal readonly bool ShowHttpStreamsWarning;

        private static readonly Encoding _utf8 = new UTF8Encoding(false);

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

        public T Deserialize<T>(ReadOnlyMemory<byte> json)
            where T : class
        {
            try
            {
                using (var stream = new ReadOnlyMemoryStream(json))
                using (var textReader = new StreamReader(stream, _utf8))
                using (var jsonReader = new JsonTextReader(textReader))
                {
                    if (Library.Debug.DumpJson)
                    {
                        Library.Debug.DumpWriter.WriteLine(_utf8.GetString(json.Span));
                    }

                    return UnderlyingSerializer.Deserialize<T>(jsonReader);
                }
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException(true, json, typeof(T), ex);
            }
        }

        public Memory<byte> Serialize(object model)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream, _utf8))
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    UnderlyingSerializer.Serialize(jsonWriter, JToken.FromObject(model, UnderlyingSerializer));

                    jsonWriter.Flush();
                    memoryStream.TryGetBuffer(out var streamBuffer);
                    if (Library.Debug.DumpJson)
                    {
                        Library.Debug.DumpWriter.WriteLine(_utf8.GetString(streamBuffer));
                    }

                    return streamBuffer;
                }
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException(false, default, model.GetType(), ex);
            }
        }

        // TODO: Temporarily horrible.
        public T? StringToEnum<T>(string value)
            where T : Enum
            => JToken.FromObject(value, UnderlyingSerializer).ToObject<T>(UnderlyingSerializer);

        public IJsonToken GetJsonToken(object value)
            => DefaultJsonToken.Create(JToken.FromObject(value, UnderlyingSerializer), UnderlyingSerializer);

        public void Dispose()
        { }
    }
}
