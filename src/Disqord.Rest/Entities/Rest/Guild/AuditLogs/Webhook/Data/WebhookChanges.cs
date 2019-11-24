using Disqord.Logging;
using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class WebhookChanges
    {
        public AuditLogChange<string> Name { get; }

        public AuditLogChange<WebhookType> Type { get; } // TODO: this might only be create/remove

        public AuditLogChange<string> AvatarHash { get; }

        public AuditLogChange<Snowflake> ChannelId { get; }

        internal WebhookChanges(RestDiscordClient client, AuditLogEntryModel model)
        {
            for (var i = 0; i < model.Changes.Length; i++)
            {
                var change = model.Changes[i];
                switch (change.Key)
                {
                    case "name":
                    {
                        Name = AuditLogChange<string>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "type":
                    {
                        Type = AuditLogChange<WebhookType>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "avatar_hash":
                    {
                        AvatarHash = AuditLogChange<string>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "channel_id":
                    {
                        ChannelId = AuditLogChange<Snowflake>.DoubleConvert<ulong>(change, client.Serializer, x => x);
                        break;
                    }

                    default:
                    {
                        client.Log(LogMessageSeverity.Error, $"Unknown change key for {nameof(WebhookChanges)}: '{change.Key}'.");
                        break;
                    }
                }
            }
        }
    }
}
