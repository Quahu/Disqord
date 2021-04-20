namespace Disqord.AuditLogs
{
    public interface IMessageUnpinnedAuditLog : IAuditLog
    {
        Snowflake ChannelId { get; }

        Snowflake MessageId { get; }
    }
}
