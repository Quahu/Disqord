using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class StickerJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("pack_id")]
        public Optional<Snowflake> PackId;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("tags")]
        public string Tags;

        [JsonProperty("type")]
        public StickerType Type;

        [JsonProperty("format_type")]
        public StickerFormatType FormatType;

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
