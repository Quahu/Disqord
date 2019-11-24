using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestBotAddedAuditLog : RestAuditLog
    {
        internal RestBotAddedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        { }
    }
}
