using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class EmojiData
    {
        public Optional<string> Name { get; }

        internal EmojiData(RestDiscordClient client, AuditLogEntryModel model, bool isCreated)
        {
            var changes = new EmojiChanges(client, model);
            if (isCreated)
            {
                Name = changes.Name.NewValue;
            }
            else
            {
                Name = changes.Name.OldValue;
            }
        }
    }
}
