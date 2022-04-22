using System;
using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs
{
    public class TransientThreadAuditLogData : IThreadAuditLogData
    {
        public Optional<string> Name { get; }

        public Optional<bool> IsArchived { get; }

        public Optional<bool> IsLocked { get; }

        public Optional<TimeSpan> AutomaticArchiveDuration { get; }

        public Optional<TimeSpan> Slowmode { get; }

        public Optional<ChannelType> Type { get; }

        public TransientThreadAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
        {
            var changes = new TransientThreadAuditLogChanges(client, model);
            if (isCreated)
            {
                Name = changes.Name.NewValue;
                IsArchived = changes.IsArchived.NewValue;
                IsLocked = changes.IsLocked.NewValue;
                AutomaticArchiveDuration = changes.AutomaticArchiveDuration.NewValue;
                Slowmode = changes.Slowmode.NewValue;
                Type = changes.Type.NewValue;
            }
            else
            {
                Name = changes.Name.OldValue;
                IsArchived = changes.IsArchived.OldValue;
                IsLocked = changes.IsLocked.OldValue;
                AutomaticArchiveDuration = changes.AutomaticArchiveDuration.OldValue;
                Slowmode = changes.Slowmode.OldValue;
                Type = changes.Type.OldValue;
            }
        }
    }
}
