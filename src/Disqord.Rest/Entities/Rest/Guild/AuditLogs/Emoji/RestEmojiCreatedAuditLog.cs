using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestEmojiCreatedAuditLog : RestAuditLog
    {
        public EmojiData Data { get; }

        internal RestEmojiCreatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Data = new EmojiData(client, entry, true);
        }
    }
}
