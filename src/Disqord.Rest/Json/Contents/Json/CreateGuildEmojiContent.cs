using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class CreateGuildEmojiContent : JsonRequestContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public LocalAttachment Image { get; set; }

        [JsonProperty("roles", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ulong> RoleIds { get; set; }
    }
}
