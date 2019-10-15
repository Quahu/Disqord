using System;
using Disqord.Models.Dispatches;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class WebSocketGuildModel : GuildModel
    {
        [JsonProperty("joined_at")]
        public DateTimeOffset? JoinedAt { get; set; }

        [JsonProperty("large")]
        public bool Large { get; set; }

        [JsonProperty("unavailable")]
        public Optional<bool> Unavailable { get; set; }

        [JsonProperty("member_count")]
        public int MemberCount { get; set; }

        [JsonProperty("voice_states")]
        public VoiceStateModel[] VoiceStates { get; set; }

        [JsonProperty("members")]
        public MemberModel[] Members { get; set; }

        [JsonProperty("channels")]
        public ChannelModel[] Channels { get; set; }

        [JsonProperty("presences")]
        public PresenceUpdateModel[] Presences { get; set; }
    }
}
