namespace Disqord.AuditLogs
{
    /// <summary>
    ///     Represents an audit log with a set of changes for the given object.
    /// </summary>
    /// <typeparam name="T"> The type of the changes. </typeparam>
    public interface IChangesAuditLog<T> : IAuditLog
    {
        /// <summary>
        ///     Gets the audit log changes for the given object.
        /// </summary>
        T Changes { get; }
    }
}
