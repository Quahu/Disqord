namespace Disqord.AuditLogs
{
    public interface IMembersMovedAuditLog : IAuditLog
    {
        /// <summary>
        ///     Gets the ID of the channel to which the members were moved.
        /// </summary>
        public Snowflake ChannelId { get; }

        /// <summary>
        ///     Gets the count of members which were moved.
        /// </summary>
        public int Count { get; }
    }
}
