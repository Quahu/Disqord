using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestGuildUpdatedAuditLog : RestAuditLog
    {
        public RestAuditLogGuildMetadata Metadata { get; }

        internal RestGuildUpdatedAuditLog(RestDiscordClient client, AuditLogModel auditLogModel, AuditLogEntryModel model) : base(client, auditLogModel, model)
        {
            Metadata = new RestAuditLogGuildMetadata(client, auditLogModel, model);
        }
    }
}