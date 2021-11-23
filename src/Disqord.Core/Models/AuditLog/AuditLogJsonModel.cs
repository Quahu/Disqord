using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class AuditLogJsonModel : JsonModel
    {
        [JsonProperty("audit_log_entries")]
        public AuditLogEntryJsonModel[] AuditLogEntries;

        [JsonProperty("guild_scheduled_events")]
        public GuildScheduledEventJsonModel[] GuildScheduledEvents;

        [JsonProperty("integrations")]
        public IntegrationJsonModel[] Integrations;

        [JsonProperty("threads")]
        public ChannelJsonModel[] Threads;

        [JsonProperty("users")]
        public UserJsonModel[] Users;

        [JsonProperty("webhooks")]
        public WebhookJsonModel[] Webhooks;
    }
}
