namespace Disqord.AuditLogs;

public interface IAutoModerationRuleUpdatedAuditLog : IChangesAuditLog<IAutoModerationRuleAuditLogChanges>, ITargetedAuditLog<IAutoModerationRule>
{ }