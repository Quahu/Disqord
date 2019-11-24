using Disqord.Logging;
using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class EmojiChanges
    {
        public AuditLogChange<string> Name { get; }

        internal EmojiChanges(RestDiscordClient client, AuditLogEntryModel model)
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
