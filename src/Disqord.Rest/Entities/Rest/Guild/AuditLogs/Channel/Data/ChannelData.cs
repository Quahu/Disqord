using System.Collections.Generic;
using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class ChannelData
    {
        public Optional<string> Name { get; }

        public Optional<string> Topic { get; }

        public Optional<int> Bitrate { get; }

        public Optional<IReadOnlyList<RestOverwrite>> Overwrites { get; }

        public Optional<bool> IsNsfw { get; }

        public Optional<int> Slowmode { get; }

        public Optional<AuditLogChannelType> Type { get; }

        internal ChannelData(RestDiscordClient client, AuditLogEntryModel model, bool isCreated)
        {
            var changes = new ChannelChanges(client, model);
            if (isCreated)
            {
                Name = changes.Name.NewValue;
                Topic = changes.Topic.NewValue;
                Bitrate = changes.Bitrate.NewValue;
                Overwrites = changes.Overwrites.NewValue;
                IsNsfw = changes.IsNsfw.NewValue;
                Slowmode = changes.Slowmode.NewValue;
                Type = changes.Type.NewValue;
            }
            else
            {
                Name = changes.Name.OldValue;
                Topic = changes.Topic.OldValue;
                Bitrate = changes.Bitrate.OldValue;
                Overwrites = changes.Overwrites.OldValue;
                IsNsfw = changes.IsNsfw.OldValue;
                Slowmode = changes.Slowmode.OldValue;
                Type = changes.Type.OldValue;
            }
        }
    }
}
