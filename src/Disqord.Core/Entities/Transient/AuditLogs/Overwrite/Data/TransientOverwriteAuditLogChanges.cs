using Disqord.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.AuditLogs;

public class TransientOverwriteAuditLogChanges : IOverwriteAuditLogChanges
{
    /// <inheritdoc/>
    public AuditLogChange<Snowflake> TargetId { get; }

    /// <inheritdoc/>
    public AuditLogChange<OverwriteTargetType> TargetType { get; }

    /// <inheritdoc/>
    public AuditLogChange<ChannelPermissions> Allowed { get; }

    /// <inheritdoc/>
    public AuditLogChange<ChannelPermissions> Denied { get; }

    public TransientOverwriteAuditLogChanges(IClient client, AuditLogEntryJsonModel model)
    {
        for (var i = 0; i < model.Changes.Value.Length; i++)
        {
            var change = model.Changes.Value[i];
            switch (change.Key)
            {
                case "id":
                {
                    TargetId = AuditLogChange<Snowflake>.Convert(change);
                    break;
                }
                case "type":
                {
                    TargetType = AuditLogChange<OverwriteTargetType>.Convert(change);
                    break;
                }
                case "allow":
                {
                    Allowed = AuditLogChange<ChannelPermissions>.Convert<ulong>(change, x => x);
                    break;
                }
                case "deny":
                {
                    Denied = AuditLogChange<ChannelPermissions>.Convert<ulong>(change, x => x);
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
