namespace Disqord.AuditLogs
{
    public interface IMemberUpdatedAuditLog : IChangesAuditLog<IMemberAuditLogChanges>
    {
        /// <summary>
        ///     Gets the user this audit log is targeting.
        ///     Returns <see langword="null"/> if the user was not provided with the audit log.
        /// </summary>
        IUser User { get; }
    }
}
