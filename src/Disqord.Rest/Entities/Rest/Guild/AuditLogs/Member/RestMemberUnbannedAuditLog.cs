using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestMemberUnbannedAuditLog : RestAuditLog
    {
        internal RestMemberUnbannedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        { }
    }
}
