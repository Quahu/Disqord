using System;

namespace Disqord.AuditLogs;

public interface IIntegrationAuditLogChanges
{
    AuditLogChange<bool> EnablesEmojis { get; }

    AuditLogChange<IntegrationExpirationBehavior> ExpireBehavior { get; }

    AuditLogChange<TimeSpan> ExpireGracePeriod { get; }
}