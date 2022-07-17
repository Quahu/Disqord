using System;
using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientAutoModerationRuleCreatedAuditLog : TransientDataAuditLog<IAutoModerationRuleAuditLogData>, IAutoModerationRuleCreatedAuditLog
{
    /// <inheritdoc/>
    public override IAutoModerationRuleAuditLogData Data { get; }

    /// <inheritdoc/>
    public IAutoModerationRule? Target
    {
        get
        {
            if (_target == null)
            {
                var ruleModel = Array.Find(AuditLogJsonModel.AutoModerationRules, ruleModel => ruleModel.Id == TargetId);
                if (ruleModel != null)
                    _target = new TransientAutoModerationRule(Client, ruleModel);
            }

            return _target;
        }
    }
    private IAutoModerationRule? _target;

    public TransientAutoModerationRuleCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientAutoModerationRuleAuditLogData(client, model, true);
    }
}
