using Disqord.Serialization.Json;

namespace Disqord.Models.Slash
{
    public class SlashCommandJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("application_id")]
        public Snowflake ApplicationId;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("options")]
        public Optional<SlashCommandOptionJsonModel[]> Options;
    }
}
