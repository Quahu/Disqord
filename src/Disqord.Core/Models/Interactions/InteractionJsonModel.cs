using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class InteractionJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("application_id")]
        public Snowflake ApplicationId;

        [JsonProperty("type")]
        public InteractionType Type;

        [JsonProperty("data")]
        public Optional<ApplicationCommandInteractionDataJsonModel> Data;

        [JsonProperty("guild_id")]
        public Optional<Snowflake> GuildId;

        [JsonProperty("channel_id")]
        public Optional<Snowflake> ChannelId;

        [JsonProperty("member")]
        public Optional<MemberJsonModel> Member;

        [JsonProperty("user")]
        public Optional<UserJsonModel> User;

        [JsonProperty("token")]
        public string Token;

        [JsonProperty("int")]
        public int Version;

        [JsonProperty("message")]
        public Optional<MessageJsonModel> Message;
    }
}
