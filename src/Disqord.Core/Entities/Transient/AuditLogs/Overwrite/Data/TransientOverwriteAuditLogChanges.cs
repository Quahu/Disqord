using Disqord.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.AuditLogs;

public class TransientOverwriteAuditLogChanges : IOverwriteAuditLogChanges
{
    /// <inheritdoc/>
    public AuditLogChange<Permissions> Allowed { get; }

    /// <inheritdoc/>
    public AuditLogChange<Permissions> Denied { get; }

    public TransientOverwriteAuditLogChanges(IClient client, AuditLogEntryJsonModel model)
    {
        for (var i = 0; i < model.Changes.Value.Length; i++)
        {
            var change = model.Changes.Value[i];
            switch (change.Key)
            {
                case "allow":
                {
                    Allowed = AuditLogChange<Permissions>.Convert(change);
                    break;
                }
                case "deny":
                {
                    Denied = AuditLogChange<Permissions>.Convert(change);
                    break;
                }
                default:
                {
                    client.Logger.LogDebug("Unknown key {0} for {1}", change.Key, this);
                    break;
                }
            }
        }
    }
}
