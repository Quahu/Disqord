using System;
using Disqord.Logging;
using Disqord.Serialization.Json.Default;

namespace Disqord.Serialization.Json
{
    public interface IJsonSerializer : IDisposable
    {
        T Deserialize<T>(ReadOnlyMemory<byte> json) where T : class;

        Memory<byte> Serialize(object value);

        T StringToEnum<T>(string value) where T : Enum;

        IJsonElement GetJsonElement(object value);

        internal static IJsonSerializer CreateDefault(ILogger logger)
            => new DefaultJsonSerializer(logger);
    }
}
