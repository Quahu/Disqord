using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models
{
    public class MessageDeletedJsonModel : JsonModel
    {
        [JsonProperty("guild_id")]
        public Optional<Snowflake> GuildId;

        [JsonProperty("channel_id")]
        public Snowflake ChannelId;

        [JsonProperty("id")]
        public Snowflake Id;
    }
}
