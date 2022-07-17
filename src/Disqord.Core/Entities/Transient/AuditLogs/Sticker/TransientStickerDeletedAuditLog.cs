using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientStickerDeletedAuditLog : TransientDataAuditLog<IStickerAuditLogData>, IStickerDeletedAuditLog
{
    /// <inheritdoc/>
    public override IStickerAuditLogData Data { get; }

    public TransientStickerDeletedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientStickerAuditLogData(client, model, false);
    }
}
