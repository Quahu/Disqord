namespace Disqord.AuditLogs;

/// <summary>
///     Represents an audit log with a set of data for the given object.
/// </summary>
/// <typeparam name="TData"> The type of the data. </typeparam>
public interface IDataAuditLog<out TData> : IAuditLog
{
    /// <summary>
    ///     Gets the audit log data for the given object.
    /// </summary>
    TData Data { get; }
}
