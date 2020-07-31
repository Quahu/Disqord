using System;
using System.IO;
using System.Text;
using Disqord.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default
{
    public sealed class DefaultJsonSerializer : IJsonSerializer
    {
        internal readonly bool ShowConversionWarnings;
        internal readonly JsonSerializer _serializer;

        private readonly ILogger _logger;
        private static readonly Encoding _utf8 = new UTF8Encoding(false);

        public DefaultJsonSerializer(ILogger logger, bool showConversionWarnings = true)
        {
            _logger = logger;
            ShowConversionWarnings = showConversionWarnings;

            _serializer = new JsonSerializer
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

                    return _serializer.Deserialize<T>(jsonReader);
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
                    _serializer.Serialize(jsonWriter, model);

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
        public T StringToEnum<T>(string value)
            where T : Enum
            => JToken.FromObject(value, _serializer).ToObject<T>(_serializer);

        public IJsonElement GetJsonElement(object value)
            => new DefaultJsonElement(JToken.FromObject(value, _serializer), _serializer);

        internal void Log(LogSeverity severity, string message, Exception exception = null)
            => _logger.Log(this, new LogEventArgs("Serializer", severity, message, exception));

        public void Dispose()
        { }
    }
}
