namespace Disqord.AuditLogs
{
    public interface IMembersMovedAuditLog : IAuditLog
    {
        public Snowflake ChannelId { get; }

        public int Count { get; }
    }
}
