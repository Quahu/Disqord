using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestInviteUpdatedAuditLog : RestAuditLog
    {
        //public OverwriteChanges Changes { get; }

        internal RestInviteUpdatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            //Changes = new OverwriteChanges(client, entry);
        }
    }
}
