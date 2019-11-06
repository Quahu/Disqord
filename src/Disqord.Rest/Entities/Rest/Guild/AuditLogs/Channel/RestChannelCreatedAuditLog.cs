//using Disqord.Models;

//namespace Disqord.Rest.AuditLogs
//{
//    public sealed class RestChannelCreatedAuditLog : RestAuditLog
//    {
//        public RestAuditLogChannelMetadata Metadata { get; }

//        internal RestChannelCreatedAuditLog(RestDiscordClient client, AuditLogModel auditLogModel, AuditLogEntryModel model) : base(client, auditLogModel, model)
//        {
//            Metadata = new RestAuditLogChannelMetadata(this, auditLogModel, model);
//        }
//    }
//}