using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class MessageReferenceModel
    {
        [JsonProperty("message_id")]
        public Optional<ulong> MessageId { get; set; }

        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }

        [JsonProperty("guild_id")]
        public Optional<ulong> GuildId { get; set; }
    }
}