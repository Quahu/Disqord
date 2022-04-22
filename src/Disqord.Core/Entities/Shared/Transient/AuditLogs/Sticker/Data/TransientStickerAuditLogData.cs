using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs
{
    public class TransientStickerAuditLogData : IStickerAuditLogData
    {
        public Optional<string> Name { get; }

        public Optional<string> Description { get; }

        public Optional<string> Tags { get; }

        public Optional<StickerFormatType> FormatType { get; }

        public Optional<bool> IsAvailable { get; }

        public Optional<Snowflake> GuildId { get; }

        public TransientStickerAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
        {
            var changes = new TransientStickerAuditLogChanges(client, model);
            if (isCreated)
            {
                Name = changes.Name.NewValue;
                Description = changes.Description.NewValue;
                Tags = changes.Tags.NewValue;
                FormatType = changes.FormatType.NewValue;
                IsAvailable = changes.IsAvailable.NewValue;
                GuildId = changes.GuildId.NewValue;
            }
            else
            {
                Name = changes.Name.OldValue;
                Description = changes.Description.OldValue;
                Tags = changes.Tags.OldValue;
                FormatType = changes.FormatType.OldValue;
                IsAvailable = changes.IsAvailable.OldValue;
                GuildId = changes.GuildId.OldValue;
            }
        }
    }
}