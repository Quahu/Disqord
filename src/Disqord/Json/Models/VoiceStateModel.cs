using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class VoiceStateModel
    {
        [JsonProperty("guild_id")]
        public ulong? GuildId { get; set; }

        [JsonProperty("channel_id")]
        public Optional<ulong?> ChannelId { get; set; }

        [JsonProperty("user_id")]
        public ulong UserId { get; set; }

        [JsonProperty("member")]
        public MemberModel Member { get; set; }

        [JsonProperty("session_id")]
        public Optional<string> SessionId { get; set; }

        [JsonProperty("deaf")]
        public Optional<bool> Deaf { get; set; }

        [JsonProperty("mute")]
        public Optional<bool> Mute { get; set; }

        [JsonProperty("self_deaf")]
        public Optional<bool> SelfDeaf { get; set; }

        [JsonProperty("self_mute")]
        public Optional<bool> SelfMute { get; set; }

        [JsonProperty("self_stream")]
        public Optional<bool> SelfStream { get; set; }

        [JsonProperty("self_video")]
        public Optional<bool> SelfVideo { get; set; }

        [JsonProperty("suppress")]
        public Optional<bool> Suppress { get; set; }
    }
}
