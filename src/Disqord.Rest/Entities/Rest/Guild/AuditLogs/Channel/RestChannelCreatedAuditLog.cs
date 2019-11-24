using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestChannelCreatedAuditLog : RestAuditLog
    {
        public ChannelData Data { get; }

        internal RestChannelCreatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Data = new ChannelData(client, entry, true);
        }
    }
}
