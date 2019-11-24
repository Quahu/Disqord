using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestIntegrationDeletedAuditLog : RestAuditLog
    {
        //public OverwriteData Data { get; }

        internal RestIntegrationDeletedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            //Data = new OverwriteData(client, entry, false);
        }
    }
}
