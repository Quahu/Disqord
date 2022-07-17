using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientMessageUnpinnedAuditLog : TransientAuditLog, IMessageUnpinnedAuditLog
{
    /// <inheritdoc/>
    public Snowflake ChannelId => Model.Options.Value.ChannelId.Value;

    /// <inheritdoc/>
    public Snowflake MessageId => Model.Options.Value.MessageId.Value;

    public TransientMessageUnpinnedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    { }
}