using Disqord.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.AuditLogs
{
    public class TransientGuildEventAuditLogChanges : IGuildEventAuditLogChanges
    {
        public AuditLogChange<Snowflake> ChannelId { get; }

        public AuditLogChange<string> Name { get; }

        public AuditLogChange<string> Description { get; }

        public AuditLogChange<GuildEventTargetType> TargetType { get; }

        public AuditLogChange<string> Location { get; }

        public AuditLogChange<PrivacyLevel> PrivacyLevel { get; }

        public AuditLogChange<GuildEventStatus> Status { get; }

        public TransientGuildEventAuditLogChanges(IClient client, AuditLogEntryJsonModel model)
        {
            for (var i = 0; i < model.Changes.Value.Length; i++)
            {
                var change = model.Changes.Value[i];
                switch (change.Key)
                {
                    case "channel_id":
                    {
                        ChannelId = AuditLogChange<Snowflake>.Convert(change);
                        break;
                    }
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
                    case "entity_type":
                    {
                        TargetType = AuditLogChange<GuildEventTargetType>.Convert(change);
                        break;
                    }
                    case "location":
                    {
                        Location = AuditLogChange<string>.Convert(change);
                        break;
                    }
                    case "privacy_level":
                    {
                        PrivacyLevel = AuditLogChange<PrivacyLevel>.Convert(change);
                        break;
                    }
                    case "status":
                    {
                        Status = AuditLogChange<GuildEventStatus>.Convert(change);
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
