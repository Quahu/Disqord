using System.Collections.Generic;

namespace Disqord.AuditLogs
{
    public interface IGuildEventAuditLogChanges
    {
        AuditLogChange<IReadOnlyList<Snowflake>> SkuIds { get; }

        AuditLogChange<GuildEventTarget> EntityType { get; }

        AuditLogChange<GuildEventStatus> Status { get; }
    }
}