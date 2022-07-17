using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientGuildUpdatedAuditLog : TransientChangesAuditLog<IGuildAuditLogChanges>, IGuildUpdatedAuditLog
{
    /// <inheritdoc/>
    public override IGuildAuditLogChanges Changes { get; }

    public TransientGuildUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Changes = new TransientGuildAuditLogChanges(client, auditLogJsonModel, model);
    }
}
