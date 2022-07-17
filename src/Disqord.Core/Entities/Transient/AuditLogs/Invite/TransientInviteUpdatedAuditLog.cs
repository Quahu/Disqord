using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientInviteUpdatedAuditLog : TransientChangesAuditLog<IInviteAuditLogChanges>, IInviteUpdatedAuditLog
{
    /// <inheritdoc/>
    public override IInviteAuditLogChanges Changes { get; }

    public TransientInviteUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Changes = new TransientInviteAuditLogChanges(client, model);
    }
}
