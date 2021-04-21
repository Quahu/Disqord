using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientMessagePinnedAuditLog : TransientAuditLog, IMessagePinnedAuditLog
    {
        public Snowflake ChannelId => Model.Options.Value.ChannelId.Value;

        public Snowflake MessageId => Model.Options.Value.MessageId.Value;

        public TransientMessagePinnedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        { }
    }
}
