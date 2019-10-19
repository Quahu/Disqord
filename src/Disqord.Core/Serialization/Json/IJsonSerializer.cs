using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Disqord.Serialization.Json
{
    public interface IJsonSerializer
    {
        Encoding UTF8 { get; }

        T ToObject<T>(object value);

        T Deserialize<T>(Stream stream);

        ReadOnlyMemory<byte> Serialize(object model, IReadOnlyDictionary<string, object> additionalFields = null);
    }
}
