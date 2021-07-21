using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientStageUpdatedAuditLog : TransientChangesAuditLog<IStageAuditLogChanges>, IStageUpdatedAuditLog
    {
        public override IStageAuditLogChanges Changes { get; }

        public TransientStageUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Changes = new TransientStageAuditLogChanges(client, model);
        }
    }
}