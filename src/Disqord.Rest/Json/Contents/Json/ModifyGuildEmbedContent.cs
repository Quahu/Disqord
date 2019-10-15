using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class ModifyGuildEmbedContent : JsonRequestContent
    {
        [JsonProperty("enabled")]
        public Optional<bool> Enabled { get; set; }

        [JsonProperty("channel_id")]
        public Optional<ulong?> ChannelId { get; set; }
    }
}
