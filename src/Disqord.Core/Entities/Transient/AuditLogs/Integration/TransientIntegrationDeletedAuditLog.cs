using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientIntegrationDeletedAuditLog : TransientDataAuditLog<IIntegrationAuditLogData>, IIntegrationDeletedAuditLog
{
    /// <inheritdoc/>
    public override IIntegrationAuditLogData Data { get; }

    public TransientIntegrationDeletedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientIntegrationAuditLogData(client, model, false);
    }
}
