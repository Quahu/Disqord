namespace Disqord.AuditLogs
{
    public interface IThreadCreatedAuditLog : IDataAuditLog<IThreadAuditLogData>
    {
        /// <summary>
        ///     Gets the thread this audit log is targeting.
        ///     Returns <see langword="null"/> if the thread was not provided with the audit log.
        /// </summary>
        IThreadChannel Thread { get; }
    }
}
