using System;
using Disqord.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.AuditLogs;

public class TransientIntegrationAuditLogChanges : IIntegrationAuditLogChanges
{
    /// <inheritdoc/>
    public AuditLogChange<bool> EnablesEmojis { get; }

    /// <inheritdoc/>
    public AuditLogChange<IntegrationExpirationBehavior> ExpireBehavior { get; }

    /// <inheritdoc/>
    public AuditLogChange<TimeSpan> ExpireGracePeriod { get; }

    public TransientIntegrationAuditLogChanges(IClient client, AuditLogEntryJsonModel model)
    {
        for (var i = 0; i < model.Changes.Value.Length; i++)
        {
            var change = model.Changes.Value[i];
            switch (change.Key)
            {
                case "enable_emoticons":
                {
                    EnablesEmojis = AuditLogChange<bool>.Convert(change);
                    break;
                }
                case "expire_behavior":
                {
                    ExpireBehavior = AuditLogChange<IntegrationExpirationBehavior>.Convert(change);
                    break;
                }
                case "expire_grace_period":
                {
                    ExpireGracePeriod = AuditLogChange<TimeSpan>.Convert<int>(change, x => TimeSpan.FromDays(x));
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
