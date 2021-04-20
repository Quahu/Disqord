namespace Disqord.AuditLogs
{
    public interface IMessagesBulkDeletedAuditLog : IAuditLog
    {
        Snowflake ChannelId { get; }

        int Count { get; }
    }
}
