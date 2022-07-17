using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientAutoModerationMessageBlockedAuditLog : TransientAuditLog, IAutoModerationMessageBlockedAuditLog
{
    /// <inheritdoc/>
    public string RuleName => Model.Options.Value.AutoModerationRuleName.Value;

    /// <inheritdoc/>
    public AutoModerationRuleTrigger RuleTrigger => Model.Options.Value.AutoModerationRuleTriggerType.Value;

    /// <inheritdoc/>
    public Snowflake ChannelId => Model.Options.Value.ChannelId.Value;

    public TransientAutoModerationMessageBlockedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    { }
}
