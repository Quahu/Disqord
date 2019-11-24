using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestEmojiDeletedAuditLog : RestAuditLog
    {
        public EmojiData Data { get; }

        internal RestEmojiDeletedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Data = new EmojiData(client, entry, false);
        }
    }
}
