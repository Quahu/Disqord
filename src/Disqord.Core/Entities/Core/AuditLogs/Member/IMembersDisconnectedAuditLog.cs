namespace Disqord.AuditLogs
{
    public interface IMembersDisconnectedAuditLog : IAuditLog
    {
        /// <summary>
        ///     Gets the count of members which were disconnected.
        /// </summary>
        int Count { get; }
    }
}
