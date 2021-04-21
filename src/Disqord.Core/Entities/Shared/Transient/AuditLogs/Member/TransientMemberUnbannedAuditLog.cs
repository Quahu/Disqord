using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientMemberUnbannedAuditLog : TransientAuditLog, IMemberUnbannedAuditLog
    {
        public TransientMemberUnbannedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        { }
    }
}
