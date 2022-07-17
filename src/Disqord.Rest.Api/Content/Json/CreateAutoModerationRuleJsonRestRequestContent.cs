using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class CreateAutoModerationRuleJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("event_type")]
    public AutoModerationEventType EventType;

    [JsonProperty("trigger_type")]
    public AutoModerationRuleTrigger Trigger;

    [JsonProperty("trigger_metadata")]
    public Optional<AutoModerationTriggerMetadataJsonModel> TriggerMetadata;

    [JsonProperty("actions")]
    public AutoModerationActionJsonModel[] Actions = null!;

    [JsonProperty("enabled")]
    public Optional<bool> Enabled;

    [JsonProperty("exempt_roles")]
    public Optional<Snowflake[]> ExemptRoles;

    [JsonProperty("exempt_channels")]
    public Optional<Snowflake[]> ExemptChannels;

    protected override void OnValidate()
    {
        // TODO: Add validation when docs pr is merged
    }
}
