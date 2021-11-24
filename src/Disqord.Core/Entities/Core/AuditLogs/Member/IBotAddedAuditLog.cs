namespace Disqord.AuditLogs
{
    public interface IBotAddedAuditLog : IAuditLog
    {
        /// <summary>
        ///     Gets the bot user this audit log is targeting.
        ///     Returns <see langword="null"/> if the user was not provided with the audit log.
        /// </summary>
        IUser Bot { get; }
    }
}
