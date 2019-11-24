using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class WebhooksUpdateModel
    {
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }

        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }
    }
}
