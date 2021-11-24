using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientThreadCreatedAuditLog : TransientDataAuditLog<IThreadAuditLogData>, IThreadCreatedAuditLog
    {
        public override IThreadAuditLogData Data { get; }

        /// <inheritdoc/>
        public IThreadChannel Thread
        {
            get
            {
                if (_thread == null)
                {
                    var thread = Array.Find(AuditLogJsonModel.Threads, x => x.Id == TargetId);
                    if (thread != null)
                        _thread = new TransientThreadChannel(Client, thread);
                }

                return _thread;
            }
        }
        private IThreadChannel _thread;

        public TransientThreadCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Data = new TransientThreadAuditLogData(client, model, true);
        }
    }
}
