using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class AuditLogJsonModel : JsonModel
    {
        [JsonProperty("webhooks")]
        public WebhookJsonModel[] Webhooks = default!;

        [JsonProperty("users")]
        public UserJsonModel[] Users = default!;

        [JsonProperty("integrations")]
        public IntegrationJsonModel[] Integrations = default!;

        [JsonProperty("audit_log_entries")]
        public AuditLogEntryJsonModel[] AuditLogEntries = default!;
    }
}
