using System;
using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientAutoModerationRuleUpdatedAuditLog : TransientChangesAuditLog<IAutoModerationRuleAuditLogChanges>, IAutoModerationRuleUpdatedAuditLog
{
    /// <inheritdoc/>
    public override IAutoModerationRuleAuditLogChanges Changes { get; }

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

    public TransientAutoModerationRuleUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Changes = new TransientAutoModerationRuleAuditLogChanges(client, model);
    }
}
