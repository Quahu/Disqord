using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestOverwriteCreatedAuditLog : RestAuditLog
    {
        public OverwriteData Data { get; }

        internal RestOverwriteCreatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Data = new OverwriteData(client, entry, true);
        }
    }
}
