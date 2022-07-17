namespace Disqord.AuditLogs;

public interface IAutoModerationRuleCreatedAuditLog : IDataAuditLog<IAutoModerationRuleAuditLogData>, ITargetedAuditLog<IAutoModerationRule>
{ }