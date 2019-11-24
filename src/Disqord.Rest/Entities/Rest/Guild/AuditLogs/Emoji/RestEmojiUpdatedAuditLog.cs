using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestEmojiUpdatedAuditLog : RestAuditLog
    {
        public EmojiChanges Changes { get; }

        internal RestEmojiUpdatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Changes = new EmojiChanges(client, entry);
        }
    }
}
