using Disqord.Models;

namespace Disqord.AuditLogs;

/// <inheritdoc cref="IDataAuditLog{T}"/>
public abstract class TransientDataAuditLog<T> : TransientAuditLog, IDataAuditLog<T>
{
    /// <inheritdoc/>
    public abstract T Data { get; }

    protected TransientDataAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    { }
}