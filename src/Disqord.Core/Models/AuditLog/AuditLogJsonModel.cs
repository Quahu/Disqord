using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class AuditLogJsonModel : JsonModel
    {
        [JsonProperty("webhooks")]
        public WebhookJsonModel[] Webhooks;

        [JsonProperty("users")]
        public UserJsonModel[] Users;

        [JsonProperty("integrations")]
        public IntegrationJsonModel[] Integrations;

        [JsonProperty("audit_log_entries")]
        public AuditLogEntryJsonModel[] AuditLogEntries;
    }
}
