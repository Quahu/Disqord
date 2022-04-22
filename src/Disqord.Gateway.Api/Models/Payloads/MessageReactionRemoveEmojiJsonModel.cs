using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models
{
    public class MessageReactionRemoveEmojiJsonModel : JsonModel
    {
        [JsonProperty("channel_id")]
        public Snowflake ChannelId;

        [JsonProperty("guild_id")]
        public Optional<Snowflake> GuildId;

        [JsonProperty("message_id")]
        public Snowflake MessageId;

        [JsonProperty("emoji")]
        public EmojiJsonModel Emoji;
    }
}
