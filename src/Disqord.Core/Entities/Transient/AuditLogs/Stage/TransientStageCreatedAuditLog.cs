using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientStageCreatedAuditLog : TransientDataAuditLog<IStageAuditLogData>, IStageCreatedAuditLog
{
    /// <inheritdoc/>
    public override IStageAuditLogData Data { get; }

    public TransientStageCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientStageAuditLogData(client, model, true);
    }
}
