using Qommon;

namespace Disqord.AuditLogs;

public interface IGuildEventAuditLogData
{
    Optional<Snowflake> ChannelId { get; }

    Optional<string> Name { get; }

    Optional<string?> Description { get; }

    Optional<string?> CoverImageHash { get; }

    Optional<GuildEventTargetType> TargetType { get; }

    Optional<string?> Location { get; }

    Optional<PrivacyLevel> PrivacyLevel { get; }

    Optional<GuildEventStatus> Status { get; }
}
