namespace Disqord.AuditLogs;

/// <summary>
///     Represents an audit log with a set of changes for the given object.
/// </summary>
/// <typeparam name="TChanges"> The type of the changes. </typeparam>
public interface IChangesAuditLog<out TChanges> : IAuditLog
{
    /// <summary>
    ///     Gets the audit log changes for the given object.
    /// </summary>
    TChanges Changes { get; }
}
