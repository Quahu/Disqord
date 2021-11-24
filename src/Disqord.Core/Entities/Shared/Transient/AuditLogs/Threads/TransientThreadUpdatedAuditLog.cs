using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientThreadUpdatedAuditLog : TransientChangesAuditLog<IThreadAuditLogChanges>, IThreadUpdatedAuditLog
    {
        public override IThreadAuditLogChanges Changes { get; }

        /// <inheritdoc/>
        public IThreadChannel Thread
        {
            get
            {
                if (_thread == null)
                {
                    var threadModel = Array.Find(AuditLogJsonModel.Threads, threadModel => threadModel.Id == TargetId);
                    if (threadModel != null)
                        _thread = new TransientThreadChannel(Client, threadModel);
                }

                return _thread;
            }
        }
        private IThreadChannel _thread;

        public TransientThreadUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Changes = new TransientThreadAuditLogChanges(client, model);
        }
    }
}
