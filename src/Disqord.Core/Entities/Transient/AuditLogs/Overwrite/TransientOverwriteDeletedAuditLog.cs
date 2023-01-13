using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs;

public class TransientOverwriteDeletedAuditLog : TransientDataAuditLog<IOverwriteAuditLogData>, IOverwriteDeletedAuditLog
{
    /// <inheritdoc/>
    public override IOverwriteAuditLogData Data { get; }

    /// <inheritdoc/>
    public Snowflake OverwriteTargetId => Model.Options.Value.Id.Value;

    /// <inheritdoc/>
    public OverwriteTargetType OverwriteTargetType => Model.Options.Value.Type.Value;

    /// <inheritdoc/>
    public string? OverwriteTargetRoleName => Model.Options.Value.RoleName.GetValueOrDefault();

    public TransientOverwriteDeletedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel? auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientOverwriteAuditLogData(client, model, false);
    }
}
