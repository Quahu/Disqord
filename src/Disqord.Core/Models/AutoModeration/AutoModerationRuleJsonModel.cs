using Disqord.Serialization.Json;

namespace Disqord.Models;

public class AutoModerationRuleJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("guild_id")]
    public Snowflake GuildId;

    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("creator_id")]
    public Snowflake CreatorId;

    [JsonProperty("event_type")]
    public AutoModerationEventType EventType;

    [JsonProperty("trigger_type")]
    public AutoModerationRuleTrigger Trigger;

    [JsonProperty("trigger_metadata")]
    public AutoModerationTriggerMetadataJsonModel TriggerMetadata = null!;

    [JsonProperty("actions")]
    public AutoModerationActionJsonModel[] Actions = null!;

    [JsonProperty("enabled")]
    public bool Enabled;

    [JsonProperty("exempt_roles")]
    public Snowflake[] ExemptRoles = null!;

    [JsonProperty("exempt_channels")]
    public Snowflake[] ExemptChannels = null!;
}