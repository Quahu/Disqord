using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class GuildEmojisUpdateModel
    {
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }

        [JsonProperty("emojis")]
        public EmojiModel[] Emojis { get; set; }
    }
}
