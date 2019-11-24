using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestWebhookDeletedAuditLog : RestAuditLog
    {
        public WebhookData Data { get; }

        internal RestWebhookDeletedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Data = new WebhookData(client, entry, false);
        }
    }
}
