using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class MessageDeleteBulkModel
    {
        [JsonProperty("ids")]
        public ulong[] Ids { get; set; }

        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }

        [JsonProperty("guild_id")]
        public ulong? GuildId { get; set; }
    }
}
