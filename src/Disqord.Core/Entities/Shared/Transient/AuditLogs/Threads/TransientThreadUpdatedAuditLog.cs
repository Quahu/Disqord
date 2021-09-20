using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientThreadUpdatedAuditLog : TransientChangesAuditLog<IThreadAuditLogChanges>, IThreadUpdatedAuditLog
    {
        public override IThreadAuditLogChanges Changes { get; }

        public TransientThreadUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Changes = new TransientThreadAuditLogChanges(client, model);
        }
    }
}
