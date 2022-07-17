using System;
using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientApplicationCommandPermissionsUpdatedAuditLog : TransientChangesAuditLog<IApplicationCommandPermissionAuditLogChanges>, IApplicationCommandPermissionsUpdatedAuditLog
{
    /// <inheritdoc/>
    public override IApplicationCommandPermissionAuditLogChanges Changes { get; }

    /// <inheritdoc/>
    public Snowflake ApplicationId { get; }

    /// <inheritdoc/>
    public IApplicationCommand? Target
    {
        get
        {
            if (_target == null && TargetId != ApplicationId)
            {
                var applicationCommandModel = Array.Find(AuditLogJsonModel.ApplicationCommands, applicationCommandModel => applicationCommandModel.Id == TargetId);
                if (applicationCommandModel != null)
                    _target = new TransientApplicationCommand(Client, applicationCommandModel);
            }

            return _target;
        }
    }
    private IApplicationCommand? _target;

    public TransientApplicationCommandPermissionsUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Changes = new TransientApplicationCommandPermissionAuditLogChanges(model);
        ApplicationId = model.Options.Value.ApplicationId.Value;
    }
}
