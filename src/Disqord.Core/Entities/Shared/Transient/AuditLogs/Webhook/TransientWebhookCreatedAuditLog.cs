using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientWebhookCreatedAuditLog : TransientDataAuditLog<IWebhookAuditLogData>, IWebhookCreatedAuditLog
    {
        public override IWebhookAuditLogData Data { get; }

        public TransientWebhookCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Data = new TransientWebhookAuditLogData(client, model, true);
        }
    }
}
