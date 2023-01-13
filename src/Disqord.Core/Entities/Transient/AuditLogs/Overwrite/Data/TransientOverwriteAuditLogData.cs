using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs;

public class TransientOverwriteAuditLogData : IOverwriteAuditLogData
{
    /// <inheritdoc/>
    public Optional<Permissions> Allowed { get; }

    /// <inheritdoc/>
    public Optional<Permissions> Denied { get; }

    public TransientOverwriteAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
    {
        var changes = new TransientOverwriteAuditLogChanges(client, model);
        if (isCreated)
        {
            Allowed = changes.Allowed.NewValue;
            Denied = changes.Denied.NewValue;
        }
        else
        {
            Allowed = changes.Allowed.OldValue;
            Denied = changes.Denied.OldValue;
        }
    }
}
