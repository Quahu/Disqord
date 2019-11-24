using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestOverwriteDeletedAuditLog : RestAuditLog
    {
        public OverwriteData Data { get; }

        internal RestOverwriteDeletedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Data = new OverwriteData(client, entry, false);
        }
    }
}
