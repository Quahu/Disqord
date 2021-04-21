using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientBotAddedAuditLog : TransientAuditLog, IBotAddedAuditLog
    {
        public TransientBotAddedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        { }
    }
}
