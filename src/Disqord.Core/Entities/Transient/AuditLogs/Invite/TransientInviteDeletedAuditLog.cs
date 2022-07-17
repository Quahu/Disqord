using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientInviteDeletedAuditLog : TransientDataAuditLog<IInviteAuditLogData>, IInviteDeletedAuditLog
{
    /// <inheritdoc/>
    public override IInviteAuditLogData Data { get; }

    public TransientInviteDeletedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientInviteAuditLogData(client, model, false);
    }
}
