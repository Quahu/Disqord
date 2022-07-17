using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientMembersDisconnectedAuditLog : TransientAuditLog, IMembersDisconnectedAuditLog
{
    /// <inheritdoc/>
    public int Count => Model.Options.Value.Count.Value;

    public TransientMembersDisconnectedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    { }
}