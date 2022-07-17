using Disqord.Serialization.Json;

namespace Disqord.Models;

public class AuditLogJsonModel : JsonModel
{
    [JsonProperty("application_commands")]
    public ApplicationCommandJsonModel[] ApplicationCommands = null!;

    [JsonProperty("audit_log_entries")]
    public AuditLogEntryJsonModel[] AuditLogEntries = null!;

    [JsonProperty("auto_moderation_rules")]
    public AutoModerationRuleJsonModel[] AutoModerationRules = null!;

    [JsonProperty("guild_scheduled_events")]
    public GuildScheduledEventJsonModel[] GuildScheduledEvents = null!;

    [JsonProperty("integrations")]
    public IntegrationJsonModel[] Integrations = null!;

    [JsonProperty("threads")]
    public ChannelJsonModel[] Threads = null!;

    [JsonProperty("users")]
    public UserJsonModel[] Users = null!;

    [JsonProperty("webhooks")]
    public WebhookJsonModel[] Webhooks = null!;
}