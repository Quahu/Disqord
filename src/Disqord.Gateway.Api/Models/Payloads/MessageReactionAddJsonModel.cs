using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models
{
    public class MessageReactionAddJsonModel : JsonModel
    {
        [JsonProperty("user_id")]
        public Snowflake UserId;

        [JsonProperty("channel_id")]
        public Snowflake ChannelId;

        [JsonProperty("message_id")]
        public Snowflake MessageId;

        [JsonProperty("guild_id")]
        public Optional<Snowflake> GuildId;

        [JsonProperty("member")]
        public Optional<MemberJsonModel> Member;

        [JsonProperty("emoji")]
        public EmojiJsonModel Emoji;
    }
}
