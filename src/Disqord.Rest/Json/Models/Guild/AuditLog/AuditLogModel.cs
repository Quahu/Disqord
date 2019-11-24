using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class AuditLogModel
    {
        [JsonProperty("webhooks")]
        public WebhookModel[] Webhooks { get; set; }

        [JsonProperty("users")]
        public UserModel[] Users { get; set; }

        [JsonProperty("integrations")]
        public IntegrationModel[] Integrations { get; set; }

        [JsonProperty("audit_log_entries")]
        public AuditLogEntryModel[] AuditLogEntries { get; set; }
    }
}
