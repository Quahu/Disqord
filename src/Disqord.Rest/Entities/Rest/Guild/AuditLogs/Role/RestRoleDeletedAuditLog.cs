using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestRoleDeletedAuditLog : RestAuditLog
    {
        public RoleData Data { get; }

        internal RestRoleDeletedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Data = new RoleData(client, entry, false);
        }
    }
}
