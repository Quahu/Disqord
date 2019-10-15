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

        public byte[] Serialize(object model, IReadOnlyDictionary<string, object> additionalFields = null)
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
                return memoryStream.ToArray();
            }
        }

        public T ToObject<T>(object value)
        {
            if (value == default)
                return default;

            var jObject = JToken.FromObject(value, _serializer);
            return jObject.ToObject<T>(_serializer);
        }
    }
}
