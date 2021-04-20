using System;

namespace Disqord.AuditLogs
{
    public interface IIntegrationAuditLogData
    {
        Optional<bool> EnablesEmojis { get; }

        Optional<IntegrationExpireBehavior> ExpireBehavior { get; }

        Optional<TimeSpan> ExpireGracePeriod { get; }
    }
}
