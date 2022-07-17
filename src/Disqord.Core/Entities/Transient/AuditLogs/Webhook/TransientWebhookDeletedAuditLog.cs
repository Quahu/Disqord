using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientWebhookDeletedAuditLog : TransientDataAuditLog<IWebhookAuditLogData>, IWebhookDeletedAuditLog
{
    /// <inheritdoc/>
    public override IWebhookAuditLogData Data { get; }

    public TransientWebhookDeletedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientWebhookAuditLogData(client, model, false);
    }
}
