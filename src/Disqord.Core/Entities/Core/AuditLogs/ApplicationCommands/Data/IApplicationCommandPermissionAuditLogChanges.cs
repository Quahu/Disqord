using System.Collections.Generic;

namespace Disqord.AuditLogs;

public interface IApplicationCommandPermissionAuditLogChanges
{
    IReadOnlyList<AuditLogChange<IApplicationCommandPermission>> Permissions { get; }
}