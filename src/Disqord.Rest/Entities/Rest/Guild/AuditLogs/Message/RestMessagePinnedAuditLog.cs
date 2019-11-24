using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestMessagePinnedAuditLog : RestAuditLog
    {
        public Snowflake ChannelId { get; }

        public Snowflake MessageId { get; }

        internal RestMessagePinnedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            ChannelId = entry.Options.ChannelId;
            MessageId = entry.Options.MessageId;
        }
    }
}
