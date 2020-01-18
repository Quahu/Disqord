using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class MessageReactionRemoveEmojiModel
    {
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }

        [JsonProperty("message_id")]
        public ulong MessageId { get; set; }

        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }

        [JsonProperty("emoji")]
        public EmojiModel Emoji { get; set; }
    }
}
