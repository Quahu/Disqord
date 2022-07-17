using Disqord.Models;

namespace Disqord.AuditLogs;

/// <inheritdoc cref="IChangesAuditLog{T}"/>
public abstract class TransientChangesAuditLog<T> : TransientAuditLog, IChangesAuditLog<T>
{
    /// <inheritdoc/>
    public abstract T Changes { get; }

    protected TransientChangesAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    { }
}