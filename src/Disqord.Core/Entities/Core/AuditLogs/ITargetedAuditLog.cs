namespace Disqord.AuditLogs;

public interface ITargetedAuditLog<out TTarget> : IAuditLog
    where TTarget : ISnowflakeEntity
{
    /// <summary>
    ///     Gets the entity this audit log is targeting.
    /// </summary>
    /// <returns>
    ///     The target or <see langword="null"/> if the entity was not provided with the audit log.
    /// </returns>
    TTarget? Target { get; }
}
