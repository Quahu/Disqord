namespace Disqord.AuditLogs
{
    public interface IGuildEventAuditLogData
    {
        Optional<Snowflake> ChannelId { get; }

        Optional<string> Name { get; }

        Optional<string> Description { get; }

        Optional<GuildEventTargetType> TargetEntityType { get; }

        Optional<string> Location { get; }

        Optional<PrivacyLevel> PrivacyLevel { get; }

        Optional<GuildEventStatus> Status { get; }
    }
}
