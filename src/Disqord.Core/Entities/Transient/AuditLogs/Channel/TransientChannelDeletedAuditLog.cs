using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientChannelDeletedAuditLog : TransientDataAuditLog<IChannelAuditLogData>, IChannelDeletedAuditLog
{
    /// <inheritdoc/>
    public override IChannelAuditLogData Data { get; }

    public TransientChannelDeletedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientChannelAuditLogData(client, model, false);
    }
}
