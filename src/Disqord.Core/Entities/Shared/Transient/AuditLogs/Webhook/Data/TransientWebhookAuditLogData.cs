using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs
{
    public class TransientWebhookAuditLogData : IWebhookAuditLogData
    {
        public Optional<string> Name { get; }

        public Optional<WebhookType> Type { get; }

        public Optional<string> AvatarHash { get; }

        public Optional<Snowflake> ChannelId { get; }

        public Optional<Snowflake?> ApplicationId { get; }

        public TransientWebhookAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
        {
            var changes = new TransientWebhookAuditLogChanges(client, model);
            if (isCreated)
            {
                Name = changes.Name.NewValue;
                Type = changes.Type.NewValue;
                AvatarHash = changes.AvatarHash.NewValue;
                ChannelId = changes.ChannelId.NewValue;
                ApplicationId = changes.ApplicationId.NewValue;
            }
            else
            {
                Name = changes.Name.OldValue;
                Type = changes.Type.OldValue;
                AvatarHash = changes.AvatarHash.OldValue;
                ChannelId = changes.ChannelId.OldValue;
                ApplicationId = changes.ApplicationId.OldValue;
            }
        }
    }
}
