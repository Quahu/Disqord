using System;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models
{
    public class GatewayGuildJsonModel : GuildJsonModel
    {
        [JsonProperty("joined_at")]
        public DateTimeOffset JoinedAt;

        [JsonProperty("large")]
        public bool Large;

        [JsonProperty("unavailable")]
        public Optional<bool> Unavailable;

        [JsonProperty("member_count")]
        public int MemberCount;

        [JsonProperty("voice_states")]
        public VoiceStateJsonModel[] VoiceStates;

        [JsonProperty("members")]
        public MemberJsonModel[] Members;

        [JsonProperty("channels")]
        public ChannelJsonModel[] Channels;

        [JsonProperty("presences")]
        public PresenceJsonModel[] Presences;
    }
}
