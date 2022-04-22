using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class StickerJsonModel : StickerItemJsonModel
    {
        [JsonProperty("pack_id")]
        public Optional<Snowflake> PackId;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("tags")]
        public string Tags;

        [JsonProperty("type")]
        public StickerType Type;

        [JsonProperty("available")]
        public Optional<bool> Available;

        [JsonProperty("guild_id")]
        public Optional<Snowflake> GuildId;

        [JsonProperty("user")]
        public Optional<UserJsonModel> User;

        [JsonProperty("sort_value")]
        public Optional<int> SortValue;
    }
}
