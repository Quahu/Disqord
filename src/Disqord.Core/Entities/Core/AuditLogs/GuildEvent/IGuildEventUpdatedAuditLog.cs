namespace Disqord.AuditLogs
{
    public interface IGuildEventUpdatedAuditLog : IChangesAuditLog<IGuildEventAuditLogChanges>
    {
        /// <summary>
        ///     Gets the event this audit log is targeting.
        ///     Returns <see langword="null"/> if the event was not provided with the audit log.
        /// </summary>
        IGuildEvent Event { get; }
    }
}
