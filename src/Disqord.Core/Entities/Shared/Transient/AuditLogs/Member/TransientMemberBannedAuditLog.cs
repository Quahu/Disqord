using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientMemberBannedAuditLog : TransientAuditLog, IMemberBannedAuditLog
    {
        public TransientMemberBannedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        { }
    }
}
