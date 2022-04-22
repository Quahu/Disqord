using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs
{
    public class TransientChannelAuditLogData : IChannelAuditLogData
    {
        public Optional<string> Name { get; }

        public Optional<string> Topic { get; }

        public Optional<int> Bitrate { get; }

        public Optional<IReadOnlyList<IOverwrite>> Overwrites { get; }

        public Optional<bool> IsNsfw { get; }

        public Optional<TimeSpan> Slowmode { get; }

        public Optional<ChannelType> Type { get; }

        public Optional<string> Region { get; }

        public TransientChannelAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
        {
            var changes = new TransientChannelAuditLogChanges(client, model);
            if (isCreated)
            {
                Name = changes.Name.NewValue;
                Topic = changes.Topic.NewValue;
                Bitrate = changes.Bitrate.NewValue;
                Overwrites = changes.Overwrites.NewValue;
                IsNsfw = changes.IsNsfw.NewValue;
                Slowmode = changes.Slowmode.NewValue;
                Type = changes.Type.NewValue;
                Region = changes.Region.NewValue;
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
                Region = changes.Region.OldValue;
            }
        }
    }
}
