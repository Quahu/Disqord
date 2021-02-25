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
        public string Name = default!;

        [JsonProperty("description")]
        public string Description = default!;

        [JsonProperty("options")]
        public Optional<SlashCommandOptionJsonModel[]> Options;
    }
}
