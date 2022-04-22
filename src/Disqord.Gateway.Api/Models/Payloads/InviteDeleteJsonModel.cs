using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models
{
    public class InviteDeleteJsonModel : JsonModel
    {
        [JsonProperty("channel_id")]
        public Snowflake ChannelId;

        [JsonProperty("code")]
        public string Code;

        [JsonProperty("guild_id")]
        public Optional<Snowflake> GuildId;
    }
}
