using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestChannelUpdatedAuditLog : RestAuditLog
    {
        public ChannelChanges Changes { get; }

        internal RestChannelUpdatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Changes = new ChannelChanges(client, entry);
        }
    }
}
