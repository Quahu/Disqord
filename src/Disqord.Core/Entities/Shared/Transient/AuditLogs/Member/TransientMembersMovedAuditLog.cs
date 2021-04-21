using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientMembersMovedAuditLog : TransientAuditLog, IMembersMovedAuditLog
    {
        public Snowflake ChannelId => Model.Options.Value.ChannelId.Value;

        public int Count => Model.Options.Value.Count.Value;

        public TransientMembersMovedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        { }
    }
}
