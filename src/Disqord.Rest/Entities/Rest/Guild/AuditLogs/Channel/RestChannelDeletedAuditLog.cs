using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestChannelDeletedAuditLog : RestAuditLog
    {
        public ChannelData Data { get; }

        internal RestChannelDeletedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Data = new ChannelData(client, entry, false);
        }
    }
}
