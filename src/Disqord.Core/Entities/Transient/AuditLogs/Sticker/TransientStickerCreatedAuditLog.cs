using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientStickerCreatedAuditLog : TransientDataAuditLog<IStickerAuditLogData>, IStickerCreatedAuditLog
{
    /// <inheritdoc/>
    public override IStickerAuditLogData Data { get; }

    public TransientStickerCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientStickerAuditLogData(client, model, true);
    }
}
