//using Disqord.Models;

//namespace Disqord.Rest.AuditLogs
//{
//    public sealed class RestChannelUpdatedAuditLog : RestAuditLog
//    {
//        public RestAuditLogChannelMetadata Metadata { get; }

//        internal RestChannelUpdatedAuditLog(RestDiscordClient client, AuditLogModel auditLogModel, AuditLogEntryModel model) : base(client, auditLogModel, model)
//        {
//            Metadata = new RestAuditLogChannelMetadata(this, auditLogModel, model);
//        }
//    }
//}