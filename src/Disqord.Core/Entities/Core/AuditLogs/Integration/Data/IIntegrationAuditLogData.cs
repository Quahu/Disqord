using System;
using Qommon;

namespace Disqord.AuditLogs;

public interface IIntegrationAuditLogData
{
    Optional<bool> EnablesEmojis { get; }

    Optional<IntegrationExpirationBehavior> ExpirationBehavior { get; }

    Optional<TimeSpan> ExpirationGracePeriod { get; }
}