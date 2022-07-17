using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientRoleUpdatedAuditLog : TransientChangesAuditLog<IRoleAuditLogChanges>, IRoleUpdatedAuditLog
{
    /// <inheritdoc/>
    public override IRoleAuditLogChanges Changes { get; }

    public TransientRoleUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Changes = new TransientRoleAuditLogChanges(client, model);
    }
}
