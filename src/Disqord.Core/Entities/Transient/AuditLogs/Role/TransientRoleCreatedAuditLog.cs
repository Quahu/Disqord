using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientRoleCreatedAuditLog : TransientDataAuditLog<IRoleAuditLogData>, IRoleCreatedAuditLog
{
    /// <inheritdoc/>
    public override IRoleAuditLogData Data { get; }

    public TransientRoleCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientRoleAuditLogData(client, model, true);
    }
}
