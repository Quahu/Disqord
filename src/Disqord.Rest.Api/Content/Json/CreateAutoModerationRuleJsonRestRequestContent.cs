using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class CreateAutoModerationRuleJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("event_type")]
        public AutoModerationEventType EventType;

        [JsonProperty("trigger_type")]
        public AutoModerationRuleTriggerType TriggerType;

        [JsonProperty("trigger_metadata")]
        public AutoModerationTriggerMetadataJsonModel TriggerMetadata;

        [JsonProperty("actions")]
        public AutoModerationActionJsonModel[] Actions;

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
}
