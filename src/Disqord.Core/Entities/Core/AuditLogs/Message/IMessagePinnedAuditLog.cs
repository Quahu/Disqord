namespace Disqord.AuditLogs
{
    public interface IMessagePinnedAuditLog : IAuditLog
    {
        Snowflake ChannelId { get; }

        Snowflake MessageId { get; }
    }
}
