using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientThreadDeletedAuditLog : TransientDataAuditLog<IThreadAuditLogData>, IThreadDeletedAuditLog
{
    /// <inheritdoc/>
    public override IThreadAuditLogData Data { get; }

    public TransientThreadDeletedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientThreadAuditLogData(client, model, false);
    }
}
