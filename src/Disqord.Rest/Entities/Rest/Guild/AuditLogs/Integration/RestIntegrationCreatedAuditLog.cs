using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestIntegrationCreatedAuditLog : RestAuditLog
    {
        //public RoleData Data { get; }

        internal RestIntegrationCreatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            //Data = new RoleData(client, entry, true);
        }
    }
}
