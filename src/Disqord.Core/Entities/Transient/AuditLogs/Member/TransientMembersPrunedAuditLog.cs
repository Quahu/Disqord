using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientMembersPrunedAuditLog : TransientAuditLog, IMembersPrunedAuditLog
{
    /// <inheritdoc/>
    public int Days => Model.Options.Value.DeleteMemberDays.Value;

    /// <inheritdoc/>
    public int Count => Model.Options.Value.Count.Value;

    public TransientMembersPrunedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    { }
}