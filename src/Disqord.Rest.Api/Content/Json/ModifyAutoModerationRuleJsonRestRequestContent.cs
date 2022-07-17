using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class ModifyAutoModerationRuleJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("name")]
    public Optional<string> Name;

    [JsonProperty("event_type")]
    public Optional<AutoModerationEventType> EventType;

    [JsonProperty("trigger_metadata")]
    public Optional<AutoModerationTriggerMetadataJsonModel> TriggerMetadata;

    [JsonProperty("actions")]
    public Optional<AutoModerationActionJsonModel[]> Actions;

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