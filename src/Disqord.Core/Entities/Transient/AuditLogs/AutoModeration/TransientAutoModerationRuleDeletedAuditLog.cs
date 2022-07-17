using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientAutoModerationRuleDeletedAuditLog : TransientDataAuditLog<IAutoModerationRuleAuditLogData>, IAutoModerationRuleDeletedAuditLog
{
    /// <inheritdoc/>
    public override IAutoModerationRuleAuditLogData Data { get; }

    public TransientAutoModerationRuleDeletedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientAutoModerationRuleAuditLogData(client, model, false);
    }
}
