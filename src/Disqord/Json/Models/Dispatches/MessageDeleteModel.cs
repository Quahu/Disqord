using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class MessageDeleteModel : JsonModel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }

        [JsonProperty("guild_id")]
        public ulong? GuildId { get; set; }
    }
}
