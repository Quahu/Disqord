using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models
{
    public class AutoModerationActionExecutionJsonModel : JsonModel
    {
        [JsonProperty("guild_id")]
        public Snowflake GuildId;

        [JsonProperty("action")]
        public AutoModerationActionJsonModel Action;

        [JsonProperty("rule_id")]
        public Snowflake RuleId;

        [JsonProperty("rule_trigger_type")]
        public AutoModerationRuleTriggerType RuleTriggerType;

        [JsonProperty("user_id")]
        public Snowflake UserId;

        [JsonProperty("channel_id")]
        public Snowflake? ChannelId;

        [JsonProperty("message_id")]
        public Snowflake? MessageId;

        [JsonProperty("alert_system_message_id")]
        public Snowflake? AlertSystemMessageId;

        [JsonProperty("content")]
        public string Content;

        [JsonProperty("matched_keyword")]
        public string MatchedKeyword;

        [JsonProperty("matched_content")]
        public string MatchedContent;
    }
}
