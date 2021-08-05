using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientThreadCreatedAuditLog : TransientDataAuditLog<IThreadAuditLogData>, IThreadCreatedAuditLog
    {
        public override IThreadAuditLogData Data { get; }

        public TransientThreadCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Data = new TransientThreadAuditLogData(client, model, true);
        }
    }
}
