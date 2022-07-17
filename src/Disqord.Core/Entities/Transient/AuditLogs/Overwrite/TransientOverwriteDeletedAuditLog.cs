using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientOverwriteDeletedAuditLog : TransientDataAuditLog<IOverwriteAuditLogData>, IOverwriteDeletedAuditLog
{
    /// <inheritdoc/>
    public override IOverwriteAuditLogData Data { get; }

    public TransientOverwriteDeletedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientOverwriteAuditLogData(client, model, false);
    }
}
