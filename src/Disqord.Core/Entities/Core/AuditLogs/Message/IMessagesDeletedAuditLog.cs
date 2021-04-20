namespace Disqord.AuditLogs
{
    public interface IMessagesDeletedAuditLog : IAuditLog
    {
        Snowflake ChannelId { get; }

        int Count { get; }
    }
}
