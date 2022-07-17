using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientEmojiUpdatedAuditLog : TransientChangesAuditLog<IEmojiAuditLogChanges>, IEmojiUpdatedAuditLog
{
    /// <inheritdoc/>
    public override IEmojiAuditLogChanges Changes { get; }

    public TransientEmojiUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Changes = new TransientEmojiAuditLogChanges(client, model);
    }
}
