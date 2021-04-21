using System.Collections.Generic;

namespace Disqord.AuditLogs
{
    public interface IMemberRolesUpdatedAuditLog : IAuditLog
    {
        Optional<IReadOnlyDictionary<Snowflake, string>> RolesGranted { get; }

        Optional<IReadOnlyDictionary<Snowflake, string>> RolesRevoked { get; }
    }
}
