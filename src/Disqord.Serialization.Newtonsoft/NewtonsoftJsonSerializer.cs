using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Newtonsoft
{
    public sealed class NewtonsoftJsonSerializer : IJsonSerializer
    {
        public static readonly NewtonsoftJsonSerializer Instance = new NewtonsoftJsonSerializer();

        private readonly JsonSerializer _serializer;

        public Encoding UTF8 { get; } = new UTF8Encoding(false, true);

        private NewtonsoftJsonSerializer()
        {
            _serializer = new JsonSerializer
            {
                ContractResolver = DiscordContractResolver.Instance
            };
        }

        public T Deserialize<T>(Stream stream)
        {
            try
            {
                using (var streamReader = new StreamReader(stream, UTF8, leaveOpen: true))
                using (var jsonReader = new JsonTextReader(streamReader))
                {
#if DEBUG && false
                    var jObject = JToken.Load(jsonReader);
                    Console.WriteLine(jObject);
                    return jObject.ToObject<T>(_serializer);
#else
                    return _serializer.Deserialize<T>(jsonReader);
#endif
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException("An exception occured for Deserialize.", ex);
            }
        }

        public ReadOnlyMemory<byte> Serialize(object model, IReadOnlyDictionary<string, object> additionalFields = null)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream, UTF8))
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    if (additionalFields == null)
                    {
                        _serializer.Serialize(jsonWriter, model);
                    }
                    else
                    {
                        var jObject = JObject.FromObject(model, _serializer);

                        foreach (var kvp in additionalFields)
                            jObject.Add(kvp.Key, JToken.FromObject(kvp.Value));

                        _serializer.Serialize(jsonWriter, jObject);
                    }

                    jsonWriter.Flush();
                    memoryStream.TryGetBuffer(out var streamBuffer);
                    return streamBuffer.AsMemory(0, (int) memoryStream.Length);
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException("An exception occured for Serialize.", ex);
            }
        }

        public T ToObject<T>(object value)
        {
            try
            {
                if (value == null || value is T tValue && tValue == default)
                    return default;

                var jObject = JToken.FromObject(value, _serializer);
                return jObject.ToObject<T>(_serializer);
            }
            catch (Exception ex)
            {
                throw new SerializationException("A serialization exception occurred for ToObject conversion.", ex);
            }
        }
    }
}
