using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientChannelUpdatedAuditLog : TransientChangesAuditLog<IChannelAuditLogChanges>, IChannelUpdatedAuditLog
{
    /// <inheritdoc/>
    public override IChannelAuditLogChanges Changes { get; }

    public TransientChannelUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Changes = new TransientChannelAuditLogChanges(client, model);
    }
}
