using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestGuildUpdatedAuditLog : RestAuditLog
    {
        public GuildChanges Changes { get; }

        internal RestGuildUpdatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Changes = new GuildChanges(client, log, entry);
        }
    }
}
