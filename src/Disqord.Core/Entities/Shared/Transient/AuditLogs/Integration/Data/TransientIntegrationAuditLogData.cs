using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientIntegrationAuditLogData : IIntegrationAuditLogData
    {
        public Optional<bool> EnablesEmojis { get; }

        public Optional<IntegrationExpireBehavior> ExpireBehavior { get; }

        public Optional<TimeSpan> ExpireGracePeriod { get; }

        public TransientIntegrationAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
        {
            var changes = new TransientIntegrationAuditLogChanges(client, model);
            if (isCreated)
            {
                EnablesEmojis = changes.EnablesEmojis.NewValue;
                ExpireBehavior = changes.ExpireBehavior.NewValue;
                ExpireGracePeriod = changes.ExpireGracePeriod.NewValue;
            }
            else
            {
                EnablesEmojis = changes.EnablesEmojis.OldValue;
                ExpireBehavior = changes.ExpireBehavior.OldValue;
                ExpireGracePeriod = changes.ExpireGracePeriod.OldValue;
            }
        }
    }
}
