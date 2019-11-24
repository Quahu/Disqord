using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class WebhookData
    {
        public Optional<string> Name { get; }

        public Optional<WebhookType> Type { get; }

        public Optional<string> AvatarHash { get; }

        public Optional<Snowflake> ChannelId { get; }

        internal WebhookData(RestDiscordClient client, AuditLogEntryModel model, bool isCreated)
        {
            var changes = new WebhookChanges(client, model);
            if (isCreated)
            {
                Name = changes.Name.NewValue;
                Type = changes.Type.NewValue;
                AvatarHash = changes.AvatarHash.NewValue;
                ChannelId = changes.ChannelId.NewValue;
            }
            else
            {
                Name = changes.Name.OldValue;
                Type = changes.Type.OldValue;
                AvatarHash = changes.AvatarHash.OldValue;
                ChannelId = changes.ChannelId.OldValue;
            }
        }
    }
}
