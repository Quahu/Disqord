using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class ModifyGuildEmojiContent : JsonRequestContent
    {
        [JsonProperty("name")]
        public Optional<string> Name { get; set; }

        [JsonProperty("roles")]
        public Optional<IReadOnlyList<ulong>> RoleIds { get; set; }
    }
}
