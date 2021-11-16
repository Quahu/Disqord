using System.Collections.Generic;

namespace Disqord.AuditLogs
{
    public interface IGuildEventAuditLogChanges
    {
        AuditLogChange<IReadOnlyList<Snowflake>> SkuIds { get; }

        AuditLogChange<GuildEventTargetType> EntityType { get; }

        AuditLogChange<GuildEventStatus> Status { get; }
    }
}