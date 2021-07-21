using Disqord.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.AuditLogs
{
    public class TransientStickerAuditLogChanges : IStickerAuditLogChanges
    {
        public AuditLogChange<string> Name { get; }

        public AuditLogChange<string> Description { get; }

        public AuditLogChange<string> Tags { get; }

        public AuditLogChange<StickerFormatType> FormatType { get; }

        public AuditLogChange<bool> IsAvailable { get; }

        public AuditLogChange<Snowflake> GuildId { get; }

        public TransientStickerAuditLogChanges(IClient client, AuditLogEntryJsonModel model)
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
                    case "description":
                    {
                        Description = AuditLogChange<string>.Convert(change);
                        break;
                    }
                    case "tags":
                    {
                        Tags = AuditLogChange<string>.Convert(change);
                        break;
                    }
                    case "format_type":
                    {
                        FormatType = AuditLogChange<StickerFormatType>.Convert(change);
                        break;
                    }
                    case "available":
                    {
                        IsAvailable = AuditLogChange<bool>.Convert(change);
                        break;
                    }
                    case "guild_id":
                    {
                        GuildId = AuditLogChange<Snowflake>.Convert(change);
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
}