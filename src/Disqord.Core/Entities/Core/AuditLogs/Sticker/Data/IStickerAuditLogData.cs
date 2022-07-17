using Qommon;

namespace Disqord.AuditLogs;

public interface IStickerAuditLogData
{
    Optional<string> Name { get; }

    Optional<string> Description { get; }

    Optional<string> Tags { get; }

    Optional<StickerFormatType> FormatType { get; }

    Optional<bool> IsAvailable { get; }

    Optional<Snowflake> GuildId { get; }
}