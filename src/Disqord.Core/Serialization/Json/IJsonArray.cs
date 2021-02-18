using System.Collections.Generic;

namespace Disqord.Serialization.Json
{
    public interface IJsonArray : IJsonToken, IEnumerable<IJsonToken>
    {
        int Count { get; }

        IJsonToken this[int index] { get; }
    }
}
