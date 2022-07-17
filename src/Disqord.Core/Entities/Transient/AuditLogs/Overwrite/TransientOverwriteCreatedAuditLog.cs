using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientOverwriteCreatedAuditLog : TransientDataAuditLog<IOverwriteAuditLogData>, IOverwriteCreatedAuditLog
{
    /// <inheritdoc/>
    public override IOverwriteAuditLogData Data { get; }

    public TransientOverwriteCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientOverwriteAuditLogData(client, model, true);
    }
}
