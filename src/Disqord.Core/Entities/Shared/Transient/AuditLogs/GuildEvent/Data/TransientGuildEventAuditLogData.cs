using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientGuildEventAuditLogData : IGuildEventAuditLogData
    {
        public Optional<Snowflake> ChannelId { get; }

        public Optional<string> Name { get; }

        public Optional<string> Description { get; }

        public Optional<GuildEventTargetType> TargetType { get; }

        public Optional<string> Location { get; }

        public Optional<PrivacyLevel> PrivacyLevel { get; }

        public Optional<GuildEventStatus> Status { get; }

        public TransientGuildEventAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
        {
            var changes = new TransientGuildEventAuditLogChanges(client, model);
            if (isCreated)
            {
                ChannelId = changes.ChannelId.NewValue;
                Name = changes.Name.NewValue;
                Description = changes.Description.NewValue;
                TargetType = changes.TargetType.NewValue;
                Location = changes.Location.NewValue;
                PrivacyLevel = changes.PrivacyLevel.NewValue;
                Status = changes.Status.NewValue;
            }
            else
            {
                ChannelId = changes.ChannelId.OldValue;
                Name = changes.Name.OldValue;
                Description = changes.Description.OldValue;
                TargetType = changes.TargetType.OldValue;
                Location = changes.Location.OldValue;
                PrivacyLevel = changes.PrivacyLevel.OldValue;
                Status = changes.Status.OldValue;
            }
        }
    }
}
