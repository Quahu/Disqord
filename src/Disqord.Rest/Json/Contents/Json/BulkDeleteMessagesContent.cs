using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class BulkDeleteMessagesContent : JsonRequestContent
    {
        [JsonProperty("messages")]
        public IEnumerable<ulong> Messages { get; set; }
    }
}
