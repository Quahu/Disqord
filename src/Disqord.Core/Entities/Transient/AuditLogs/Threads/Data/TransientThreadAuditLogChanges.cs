using System;
using Disqord.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.AuditLogs;

public class TransientThreadAuditLogChanges : IThreadAuditLogChanges
{
    /// <inheritdoc/>
    public AuditLogChange<string> Name { get; }

    /// <inheritdoc/>
    public AuditLogChange<bool> IsArchived { get; }

    /// <inheritdoc/>
    public AuditLogChange<bool> IsLocked { get; }

    /// <inheritdoc/>
    public AuditLogChange<TimeSpan> AutomaticArchiveDuration { get; }

    /// <inheritdoc/>
    public AuditLogChange<TimeSpan> Slowmode { get; }

    /// <inheritdoc/>
    public AuditLogChange<ChannelType> Type { get; }

    public TransientThreadAuditLogChanges(IClient client, AuditLogEntryJsonModel model)
    {
        for (var i = 0; i < model.Changes.Value.Length; i++)
        {
            var change = model.Changes.Value[i];
            switch (change.Key)
            {
                case "name":
                {
                    Name = AuditLogChange<string>.Convert(change);
                    break;
                }
                case "archived":
                {
                    IsArchived = AuditLogChange<bool>.Convert(change);
                    break;
                }
                case "locked":
                {
                    IsLocked = AuditLogChange<bool>.Convert(change);
                    break;
                }
                case "auto_archive_duration":
                {
                    AutomaticArchiveDuration = AuditLogChange<TimeSpan>.Convert<int>(change, x => TimeSpan.FromMinutes(x));
                    break;
                }
                case "rate_limit_per_user":
                {
                    Slowmode = AuditLogChange<TimeSpan>.Convert<int>(change, x => TimeSpan.FromSeconds(x));
                    break;
                }
                case "type":
                {
                    Type = AuditLogChange<ChannelType>.Convert(change);
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
