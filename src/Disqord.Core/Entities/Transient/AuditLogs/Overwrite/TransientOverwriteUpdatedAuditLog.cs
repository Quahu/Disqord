using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientOverwriteUpdatedAuditLog : TransientChangesAuditLog<IOverwriteAuditLogChanges>, IOverwriteUpdatedAuditLog
{
    /// <inheritdoc/>
    public override IOverwriteAuditLogChanges Changes { get; }

    public TransientOverwriteUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Changes = new TransientOverwriteAuditLogChanges(client, model);
    }
}
