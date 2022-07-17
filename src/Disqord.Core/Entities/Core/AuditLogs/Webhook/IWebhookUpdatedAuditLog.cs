namespace Disqord.AuditLogs;

public interface IWebhookUpdatedAuditLog : IChangesAuditLog<IWebhookAuditLogChanges>, ITargetedAuditLog<IWebhook>
{ }