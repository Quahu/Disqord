using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientGuildEventCreatedAuditLog : TransientDataAuditLog<IGuildEventAuditLogData>, IGuildEventCreatedAuditLog
    {
        public override IGuildEventAuditLogData Data { get; }

        public TransientGuildEventCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Data = new TransientGuildEventAuditLogData(client, model, true);
        }
    }
}
