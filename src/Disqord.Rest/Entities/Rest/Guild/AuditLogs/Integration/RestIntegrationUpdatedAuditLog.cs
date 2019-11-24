using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestIntegrationUpdatedAuditLog : RestAuditLog
    {
        //public OverwriteChanges Changes { get; }

        internal RestIntegrationUpdatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            //Changes = new OverwriteChanges(client, entry);
        }
    }
}
