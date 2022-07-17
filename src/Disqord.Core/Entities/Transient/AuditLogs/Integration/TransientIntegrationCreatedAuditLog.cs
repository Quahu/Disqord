using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientIntegrationCreatedAuditLog : TransientDataAuditLog<IIntegrationAuditLogData>, IIntegrationCreatedAuditLog
{
    /// <inheritdoc/>
    public override IIntegrationAuditLogData Data { get; }

    public TransientIntegrationCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientIntegrationAuditLogData(client, model, true);
    }
}
