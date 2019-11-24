using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestRoleCreatedAuditLog : RestAuditLog
    {
        public RoleData Data { get; }

        internal RestRoleCreatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Data = new RoleData(client, entry, true);
        }
    }
}
