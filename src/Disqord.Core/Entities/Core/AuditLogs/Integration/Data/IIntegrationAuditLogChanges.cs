using System;

namespace Disqord.AuditLogs
{
    public interface IIntegrationAuditLogChanges
    {
        AuditLogChange<bool> EnablesEmojis { get; }

        AuditLogChange<IntegrationExpireBehavior> ExpireBehavior { get; }

        AuditLogChange<TimeSpan> ExpireGracePeriod { get; }
    }
}
