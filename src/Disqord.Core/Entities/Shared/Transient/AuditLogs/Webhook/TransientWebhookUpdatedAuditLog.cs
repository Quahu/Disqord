using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientWebhookUpdatedAuditLog : TransientChangesAuditLog<IWebhookAuditLogChanges>, IWebhookUpdatedAuditLog
    {
        public override IWebhookAuditLogChanges Changes { get; }

        public TransientWebhookUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Changes = new TransientWebhookAuditLogChanges(client, model);
        }
    }
}
