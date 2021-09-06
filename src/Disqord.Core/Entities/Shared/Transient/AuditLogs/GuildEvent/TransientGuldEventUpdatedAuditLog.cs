using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientGuldEventUpdatedAuditLog : TransientChangesAuditLog<IGuildEventAuditLogChanges>, IGuildEventUpdatedAuditLog
    {
        public override IGuildEventAuditLogChanges Changes { get; }

        public TransientGuldEventUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Changes = new TransientGuildEventAuditLogChanges(client, model);
        }
    }
}