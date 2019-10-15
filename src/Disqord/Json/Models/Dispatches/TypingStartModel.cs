using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class TypingStartModel
    {
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }

        [JsonProperty("guild_id")]
        public ulong? GuildId { get; set; }

        [JsonProperty("user_id")]
        public ulong UserId { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
