using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientUnknownAuditLog : TransientAuditLog, IUnknownAuditLog
{
    /// <inheritdoc/>
    public AuditLogActionType Type => Model.ActionType;

    public TransientUnknownAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    { }
}