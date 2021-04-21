namespace Disqord.AuditLogs
{
    /// <summary>
    ///     Represents an audit log with a set of data for the given object.
    /// </summary>
    /// <typeparam name="T"> The type of the data. </typeparam>
    public interface IDataAuditLog<T> : IAuditLog
    {
        /// <summary>
        ///     Gets the audit log data for the given object.
        /// </summary>
        T Data { get; }
    }
}
