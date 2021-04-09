using System;
using Disqord.Logging;

namespace Disqord.Serialization.Json
{
    public interface IJsonSerializer : ILogging, IDisposable
    {
        T Deserialize<T>(ReadOnlyMemory<byte> json)
            where T : class;

        Memory<byte> Serialize(object value);

        T StringToEnum<T>(string value)
            where T : Enum;

        IJsonToken GetJsonToken(object value);
    }
}
