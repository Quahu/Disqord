using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestMessagesBulkDeletedAuditLog : RestAuditLog
    {
        public Snowflake ChannelId { get; }

        public int Count { get; }

        internal RestMessagesBulkDeletedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            ChannelId = entry.Options.ChannelId;
            Count = entry.Options.Count;
        }
    }
}
