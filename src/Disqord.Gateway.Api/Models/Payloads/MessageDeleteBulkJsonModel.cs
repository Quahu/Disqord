using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models
{
    public class MessageDeleteBulkJsonModel : JsonModel
    {
        [JsonProperty("guild_id")]
        public Optional<Snowflake> GuildId;

        [JsonProperty("channel_id")]
        public Snowflake ChannelId;

        [JsonProperty("ids")]
        public Snowflake[] Ids;
    }
}
