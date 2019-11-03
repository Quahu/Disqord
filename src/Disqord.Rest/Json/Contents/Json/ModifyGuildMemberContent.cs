using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class ModifyGuildMemberContent : JsonRequestContent
    {
        [JsonProperty("nick")]
        public Optional<string> Nick { get; set; }

        [JsonProperty("roles")]
        public Optional<IReadOnlyList<ulong>> RoleIds { get; set; }

        [JsonProperty("mute")]
        public Optional<bool> Mute { get; set; }

        [JsonProperty("deaf")]
        public Optional<bool> Deaf { get; set; }

        [JsonProperty("channel_id")]
        public Optional<ulong?> VoiceChannelId { get; set; }
    }
}
