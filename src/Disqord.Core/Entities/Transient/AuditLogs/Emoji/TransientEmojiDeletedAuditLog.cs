using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientEmojiDeletedAuditLog : TransientDataAuditLog<IEmojiAuditLogData>, IEmojiDeletedAuditLog
{
    /// <inheritdoc/>
    public override IEmojiAuditLogData Data { get; }

    public TransientEmojiDeletedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientEmojiAuditLogData(client, model, false);
    }
}
