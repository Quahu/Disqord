using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestRoleUpdatedAuditLog : RestAuditLog
    {
        public RoleChanges Changes { get; }

        internal RestRoleUpdatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            Changes = new RoleChanges(client, entry);
        }
    }
}
