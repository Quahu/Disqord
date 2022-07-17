using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord.AuditLogs;

public class TransientApplicationCommandPermissionAuditLogChanges : IApplicationCommandPermissionAuditLogChanges
{
    /// <inheritdoc/>
    public IReadOnlyList<AuditLogChange<IApplicationCommandPermission>> Permissions { get; }

    public TransientApplicationCommandPermissionAuditLogChanges(AuditLogEntryJsonModel model)
    {
        var permissions = new List<AuditLogChange<IApplicationCommandPermission>>();
        foreach (var changeModel in model.Changes.Value)
        {
            var change = AuditLogChange<IApplicationCommandPermission>.Convert<ApplicationCommandPermissionsJsonModel>(changeModel,
                model => new TransientApplicationCommandPermission(model));

            permissions.Add(change);
        }

        Permissions = permissions.ReadOnly();
    }
}
