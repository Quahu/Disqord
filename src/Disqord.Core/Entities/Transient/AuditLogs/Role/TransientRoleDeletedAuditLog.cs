using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientRoleDeletedAuditLog : TransientDataAuditLog<IRoleAuditLogData>, IRoleDeletedAuditLog
{
    /// <inheritdoc/>
    public override IRoleAuditLogData Data { get; }

    public TransientRoleDeletedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientRoleAuditLogData(client, model, false);
    }
}
