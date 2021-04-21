using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientMemberKickedAuditLog : TransientAuditLog, IMemberKickedAuditLog
    {
        public TransientMemberKickedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        { }
    }
}
