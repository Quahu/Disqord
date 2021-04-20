using System.Collections.Generic;

namespace Disqord.AuditLogs
{
    public interface IMemberRolesUpdatedAuditLog : IAuditLog
    {
        Optional<IReadOnlyList<AuditLogRole>> RolesAdded { get; }

        Optional<IReadOnlyList<AuditLogRole>> RolesRemoved { get; }
    }
}
