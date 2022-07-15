using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord.AuditLogs
{
    public class TransientApplicationCommandPermissionAuditLogChanges : IApplicationCommandPermissionAuditLogChanges
    {
        public IReadOnlyList<AuditLogChange<IApplicationCommandPermission>> Permissions { get; }

        public TransientApplicationCommandPermissionAuditLogChanges(AuditLogEntryJsonModel model)
        {
            var permissions = new List<AuditLogChange<IApplicationCommandPermission>>();
            foreach (var change in model.Changes.Value)
            {
                permissions.Add(AuditLogChange<IApplicationCommandPermission>.Convert<ApplicationCommandPermissionsJsonModel>(change,
                    x => new TransientApplicationCommandPermission(x)));
            }
            Permissions = permissions.ReadOnly();
        }
    }
}
