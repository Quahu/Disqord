using System;
using Disqord.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.AuditLogs;

public class TransientInviteAuditLogChanges : IInviteAuditLogChanges
{
    /// <inheritdoc/>
    public AuditLogChange<string> Code { get; }

    /// <inheritdoc/>
    public AuditLogChange<Snowflake> ChannelId { get; }

    /// <inheritdoc/>
    public AuditLogChange<Snowflake> InviterId { get; }

    /// <inheritdoc/>
    public AuditLogChange<int> MaxUses { get; }

    /// <inheritdoc/>
    public AuditLogChange<int> Uses { get; }

    /// <inheritdoc/>
    public AuditLogChange<bool> IsTemporary { get; }

    /// <inheritdoc/>
    public AuditLogChange<TimeSpan> MaxAge { get; }

    public TransientInviteAuditLogChanges(IClient client, AuditLogEntryJsonModel model)
    {
        for (var i = 0; i < model.Changes.Value.Length; i++)
        {
            var change = model.Changes.Value[i];
            switch (change.Key)
            {
                case "code":
                {
                    Code = AuditLogChange<string>.Convert(change);
                    break;
                }
                case "channel_id":
                {
                    ChannelId = AuditLogChange<Snowflake>.Convert(change);
                    break;
                }
                case "inviter_id":
                {
                    InviterId = AuditLogChange<Snowflake>.Convert(change);
                    break;
                }
                case "max_uses":
                {
                    MaxUses = AuditLogChange<int>.Convert(change);
                    break;
                }
                case "uses":
                {
                    Uses = AuditLogChange<int>.Convert(change);
                    break;
                }
                case "temporary":
                {
                    IsTemporary = AuditLogChange<bool>.Convert(change);
                    break;
                }
                case "max_age":
                {
                    MaxAge = AuditLogChange<TimeSpan>.Convert<int>(change, x => TimeSpan.FromSeconds(x));
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
