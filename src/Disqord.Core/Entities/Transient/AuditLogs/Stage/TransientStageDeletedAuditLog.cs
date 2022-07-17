using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientStageDeletedAuditLog : TransientDataAuditLog<IStageAuditLogData>, IStageDeletedAuditLog
{
    /// <inheritdoc/>
    public override IStageAuditLogData Data { get; }

    public TransientStageDeletedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientStageAuditLogData(client, model, false);
    }
}
