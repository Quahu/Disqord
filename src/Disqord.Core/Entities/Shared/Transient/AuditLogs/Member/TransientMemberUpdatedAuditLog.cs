using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientMemberUpdatedAuditLog : TransientChangesAuditLog<IMemberAuditLogChanges>, IMemberUpdatedAuditLog
    {
        public override IMemberAuditLogChanges Changes { get; }

        public TransientMemberUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Changes = new TransientMemberAuditLogChanges(client, model);
        }
    }
}
