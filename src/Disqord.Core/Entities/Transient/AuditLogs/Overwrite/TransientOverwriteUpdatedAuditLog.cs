using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs;

public class TransientOverwriteUpdatedAuditLog : TransientChangesAuditLog<IOverwriteAuditLogChanges>, IOverwriteUpdatedAuditLog
{
    /// <inheritdoc/>
    public override IOverwriteAuditLogChanges Changes { get; }

    /// <inheritdoc/>
    public Snowflake OverwriteTargetId => Model.Options.Value.Id.Value;

    /// <inheritdoc/>
    public OverwriteTargetType OverwriteTargetType => Model.Options.Value.Type.Value;

    /// <inheritdoc/>
    public string? OverwriteTargetRoleName => Model.Options.Value.RoleName.GetValueOrDefault();

    public TransientOverwriteUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel? auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Changes = new TransientOverwriteAuditLogChanges(client, model);
    }
}
