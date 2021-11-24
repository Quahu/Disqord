using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientGuildEventCreatedAuditLog : TransientDataAuditLog<IGuildEventAuditLogData>, IGuildEventCreatedAuditLog
    {
        public override IGuildEventAuditLogData Data { get; }

        /// <inheritdoc/>
        public IGuildEvent Event
        {
            get
            {
                if (_event == null)
                {
                    var guildEventModel = Array.Find(AuditLogJsonModel.GuildScheduledEvents, guildEventModel => guildEventModel.Id == TargetId);
                    if (guildEventModel != null)
                        _event = new TransientGuildEvent(Client, guildEventModel);
                }

                return _event;
            }
        }
        private IGuildEvent _event;

        public TransientGuildEventCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Data = new TransientGuildEventAuditLogData(client, model, true);
        }
    }
}
