using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestInviteDeletedAuditLog : RestAuditLog
    {
        //public OverwriteData Data { get; }

        internal RestInviteDeletedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            //Data = new OverwriteData(client, entry, false);
        }
    }
}
