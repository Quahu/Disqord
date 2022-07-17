namespace Disqord.AuditLogs;

public interface IUnknownAuditLog : IAuditLog
{
    /// <summary>
    ///     Gets the type of this unknown audit log.
    /// </summary>
    AuditLogActionType Type { get; }
}