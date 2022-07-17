using System.Globalization;
using Qommon;

namespace Disqord.AuditLogs;

public interface IGuildAuditLogChanges
{
    AuditLogChange<string> Name { get; }

    AuditLogChange<string?> Description { get; }

    AuditLogChange<string?> IconHash { get; }

    AuditLogChange<string?> SplashHash { get; }

    AuditLogChange<string?> DiscoverySplashHash { get; }

    AuditLogChange<string?> BannerHash { get; }

    AuditLogChange<Snowflake> OwnerId { get; }

    AuditLogChange<Optional<IUser>> Owner { get; }

    AuditLogChange<CultureInfo> PreferredLocale { get; }

    AuditLogChange<Snowflake?> AfkChannelId { get; }

    AuditLogChange<int> AfkTimeout { get; }

    AuditLogChange<Snowflake?> RulesChannelId { get; }

    AuditLogChange<Snowflake?> PublicUpdatesChannelId { get; }

    AuditLogChange<GuildMfaLevel> MfaLevel { get; }

    AuditLogChange<GuildVerificationLevel> VerificationLevel { get; }

    AuditLogChange<GuildContentFilterLevel> ContentFilterLevel { get; }

    AuditLogChange<GuildNotificationLevel> DefaultNotificationLevel { get; }

    AuditLogChange<string?> VanityUrlCode { get; }

    AuditLogChange<int?> PruneDays { get; }

    AuditLogChange<bool> IsWidgetEnabled { get; }

    AuditLogChange<Snowflake?> WidgetChannelId { get; }

    AuditLogChange<Snowflake?> SystemChannelId { get; }
}
