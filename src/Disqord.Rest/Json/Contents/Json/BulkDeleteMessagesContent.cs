using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class BulkDeleteMessagesContent : JsonRequestContent
    {
        [JsonProperty("messages")]
        public IReadOnlyList<ulong> Messages { get; set; }
    }
}
