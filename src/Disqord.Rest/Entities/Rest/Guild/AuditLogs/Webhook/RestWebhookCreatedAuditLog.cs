using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestWebhookCreatedAuditLog : RestAuditLog
    {
        public WebhookData Data { get; }

        internal RestWebhookCreatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Data = new WebhookData(client, entry, true);
        }
    }
}
