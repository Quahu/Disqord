namespace Disqord.AuditLogs
{
    public interface ITargetedAuditLog<TTarget>
        where TTarget : ISnowflakeEntity
    {
        /// <summary>
        ///     Gets the entity this audit log is targeting.
        ///     Returns <see langword="null"/> if the entity was not provided with the audit log.
        /// </summary>
        TTarget Target { get; }
    }

}
