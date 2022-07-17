using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class AutoModerationActionExecutionJsonModel : JsonModel
{
    [JsonProperty("guild_id")]
    public Snowflake GuildId;

    [JsonProperty("action")]
    public AutoModerationActionJsonModel Action = null!;

    [JsonProperty("rule_id")]
    public Snowflake RuleId;

    [JsonProperty("rule_trigger_type")]
    public AutoModerationRuleTrigger RuleTrigger;

    [JsonProperty("user_id")]
    public Snowflake UserId;

    [JsonProperty("channel_id")]
    public Optional<Snowflake> ChannelId;

    [JsonProperty("message_id")]
    public Optional<Snowflake> MessageId;

    [JsonProperty("alert_system_message_id")]
    public Optional<Snowflake> AlertSystemMessageId;

    [JsonProperty("content")]
    public string Content = null!;

    [JsonProperty("matched_keyword")]
    public string? MatchedKeyword;

    [JsonProperty("matched_content")]
    public string? MatchedContent;
}