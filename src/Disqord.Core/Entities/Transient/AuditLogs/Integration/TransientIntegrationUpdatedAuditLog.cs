using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientIntegrationUpdatedAuditLog : TransientChangesAuditLog<IIntegrationAuditLogChanges>, IIntegrationUpdatedAuditLog
{
    /// <inheritdoc/>
    public override IIntegrationAuditLogChanges Changes { get; }

    public TransientIntegrationUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Changes = new TransientIntegrationAuditLogChanges(client, model);
    }
}
