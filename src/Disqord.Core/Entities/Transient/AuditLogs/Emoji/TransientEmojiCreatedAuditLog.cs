using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientEmojiCreatedAuditLog : TransientDataAuditLog<IEmojiAuditLogData>, IEmojiCreatedAuditLog
{
    /// <inheritdoc/>
    public override IEmojiAuditLogData Data { get; }

    public TransientEmojiCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientEmojiAuditLogData(client, model, true);
    }
}
