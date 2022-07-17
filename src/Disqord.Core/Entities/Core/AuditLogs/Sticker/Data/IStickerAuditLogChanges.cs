namespace Disqord.AuditLogs;

public interface IStickerAuditLogChanges
{
    AuditLogChange<string> Name { get; }

    AuditLogChange<string> Description { get; }

    AuditLogChange<string> Tags { get; }

    AuditLogChange<StickerFormatType> FormatType { get; }

    AuditLogChange<bool> IsAvailable { get; }

    AuditLogChange<Snowflake> GuildId { get; }
}