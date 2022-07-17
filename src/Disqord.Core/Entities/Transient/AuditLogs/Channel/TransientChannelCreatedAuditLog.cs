using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientChannelCreatedAuditLog : TransientDataAuditLog<IChannelAuditLogData>, IChannelCreatedAuditLog
{
    /// <inheritdoc/>
    public override IChannelAuditLogData Data { get; }

    public TransientChannelCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientChannelAuditLogData(client, model, true);
    }
}
