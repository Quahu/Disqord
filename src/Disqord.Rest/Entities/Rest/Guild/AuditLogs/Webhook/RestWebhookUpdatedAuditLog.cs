using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestWebhookUpdatedAuditLog : RestAuditLog
    {
        public WebhookChanges Changes { get; }

        internal RestWebhookUpdatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Changes = new WebhookChanges(client, entry);
        }
    }
}
