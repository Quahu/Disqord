using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestMemberBannedAuditLog : RestAuditLog
    {
        internal RestMemberBannedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        { }
    }
}
