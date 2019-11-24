using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestMembersPrunedAuditLog : RestAuditLog
    {
        public int Days { get; }

        public int Count { get; }

        internal RestMembersPrunedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Days = entry.Options.DeleteMemberDays;
            Count = entry.Options.Count;
        }
    }
}
