using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientStickerUpdatedAuditLog : TransientChangesAuditLog<IStickerAuditLogChanges>, IStickerUpdatedAuditLog
{
    /// <inheritdoc/>
    public override IStickerAuditLogChanges Changes { get; }

    public TransientStickerUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Changes = new TransientStickerAuditLogChanges(client, model);
    }
}
