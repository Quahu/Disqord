using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientMessagesBulkDeletedAuditLog : TransientAuditLog, IMessagesBulkDeletedAuditLog
    {
        public Snowflake ChannelId => Model.Options.Value.ChannelId.Value;

        public int Count => Model.Options.Value.Count.Value;

        public TransientMessagesBulkDeletedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        { }
    }
}
