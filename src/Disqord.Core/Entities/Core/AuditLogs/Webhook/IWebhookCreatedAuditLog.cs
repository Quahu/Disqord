namespace Disqord.AuditLogs;

public interface IWebhookCreatedAuditLog : IDataAuditLog<IWebhookAuditLogData>, ITargetedAuditLog<IWebhook>
{ }