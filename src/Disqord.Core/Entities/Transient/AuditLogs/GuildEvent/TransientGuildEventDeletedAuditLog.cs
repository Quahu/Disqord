using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientGuildEventDeletedAuditLog : TransientDataAuditLog<IGuildEventAuditLogData>, IGuildEventDeletedAuditLog
{
    /// <inheritdoc/>
    public override IGuildEventAuditLogData Data { get; }

    public TransientGuildEventDeletedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientGuildEventAuditLogData(client, model, false);
    }
}
