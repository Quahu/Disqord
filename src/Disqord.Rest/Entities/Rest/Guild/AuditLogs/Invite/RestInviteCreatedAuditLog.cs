using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestInviteCreatedAuditLog : RestAuditLog
    {
        //public RoleData Data { get; }

        internal RestInviteCreatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            //Data = new RoleData(client, entry, true);
        }
    }
}
