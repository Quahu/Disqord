using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientBotAddedAuditLog : TransientAuditLog, IBotAddedAuditLog
    {
        /// <inheritdoc/>
        public IUser Bot
        {
            get
            {
                if (_bot == null)
                {
                    var bot = Array.Find(AuditLogJsonModel.Users, x => x.Id == TargetId);
                    if (bot != null)
                        _bot = new TransientUser(Client, bot);
                }

                return _bot;
            }
        }
        private IUser _bot;

        public TransientBotAddedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        { }
    }
}
