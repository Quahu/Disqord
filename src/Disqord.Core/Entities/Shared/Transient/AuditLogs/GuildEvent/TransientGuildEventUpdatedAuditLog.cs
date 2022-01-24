using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientGuildEventUpdatedAuditLog : TransientChangesAuditLog<IGuildEventAuditLogChanges>, IGuildEventUpdatedAuditLog
    {
        public override IGuildEventAuditLogChanges Changes { get; }

        /// <inheritdoc/>
        public IGuildEvent Target
        {
            get
            {
                if (_target == null)
                {
                    var eventModel = Array.Find(AuditLogJsonModel.GuildScheduledEvents, eventModel => eventModel.Id == TargetId);
                    if (eventModel != null)
                        _target = new TransientGuildEvent(Client, eventModel);
                }

                return _target;
            }
        }
        private IGuildEvent _target;

        public TransientGuildEventUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Changes = new TransientGuildEventAuditLogChanges(client, model);
        }
    }
}
