using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientGuildEventUpdatedAuditLog : TransientChangesAuditLog<IGuildEventAuditLogChanges>, IGuildEventUpdatedAuditLog
    {
        public override IGuildEventAuditLogChanges Changes { get; }

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

        public TransientGuildEventUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Changes = new TransientGuildEventAuditLogChanges(client, model);
        }
    }
}
