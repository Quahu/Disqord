using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientAutoModerationMessageBlockedAuditLog : TransientAuditLog, IAutoModerationMessageBlockedAuditLog
    {
        public string RuleName => Model.Options.Value.AutoModerationRuleName.Value;

        public AutoModerationTriggerType TriggerType => Model.Options.Value.AutoModerationRuleTriggerType.Value;

        public Snowflake ChannelId => Model.Options.Value.ChannelId.Value;

        public TransientAutoModerationMessageBlockedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        { }
    }
}
