using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class AutoModerationRuleJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("guild_id")]
        public Snowflake GuildId;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("creator_id")]
        public Snowflake CreatorId;

        [JsonProperty("event_type")]
        public int EventType;

        [JsonProperty("trigger_type")]
        public int TriggerType;

        [JsonProperty("trigger_metadata")]
        public JsonModel TriggerMetadata;

        [JsonProperty("actions")]
        public JsonModel[] Actions;

        [JsonProperty("enabled")]
        public bool Enabled;

        [JsonProperty("exempt_roles")]
        public Snowflake[] ExemptRoles;

        [JsonProperty("exempt_channels")]
        public Snowflake[] ExemptChannels;
    }
}
