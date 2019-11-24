using Disqord.Logging;
using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestUnknownAuditLog : RestAuditLog
    {
        internal RestUnknownAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Client.Log(LogMessageSeverity.Error, $"Unknown audit log type received: {(int) entry.ActionType}.");
        }
    }
}