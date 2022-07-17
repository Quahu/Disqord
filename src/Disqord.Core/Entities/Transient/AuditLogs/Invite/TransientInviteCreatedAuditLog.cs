using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientInviteCreatedAuditLog : TransientDataAuditLog<IInviteAuditLogData>, IInviteCreatedAuditLog
{
    /// <inheritdoc/>
    public override IInviteAuditLogData Data { get; }

    public TransientInviteCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientInviteAuditLogData(client, model, true);
    }
}
