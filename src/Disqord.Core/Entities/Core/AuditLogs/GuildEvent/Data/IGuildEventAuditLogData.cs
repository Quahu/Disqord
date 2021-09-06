using System.Collections.Generic;

namespace Disqord.AuditLogs
{
    public interface IGuildEventAuditLogData
    {
        Optional<IReadOnlyList<Snowflake>> SkuIds { get; }

        Optional<GuildEventTarget> EntityType { get; }

        Optional<GuildEventStatus> Status { get; }
    }
}