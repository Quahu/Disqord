using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientGuildEventUpdatedAuditLog : TransientChangesAuditLog<IGuildEventAuditLogChanges>, IGuildEventUpdatedAuditLog
    {
        public override IGuildEventAuditLogChanges Changes { get; }

        public IGuildEvent Event
        {
            get
            {
                if (_event == null)
                {
                    var @event = Array.Find(AuditLogJsonModel.GuildScheduledEvents, x => x.Id == TargetId);
                    if (@event != null)
                        _event = new TransientGuildEvent(Client, @event);
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
