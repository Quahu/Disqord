using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestChannelDeletedAuditLog : RestAuditLog
    {
        public RestAuditLogChannelMetadata Metadata { get; }

        internal RestChannelDeletedAuditLog(RestDiscordClient client, AuditLogModel auditLogModel, AuditLogEntryModel model) : base(client, auditLogModel, model)
        {
            Metadata = new RestAuditLogChannelMetadata(client, auditLogModel, model);
        }
    }
}