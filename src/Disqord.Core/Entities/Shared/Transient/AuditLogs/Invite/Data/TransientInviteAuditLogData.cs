using System;
using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs
{
    public class TransientInviteAuditLogData : IInviteAuditLogData
    {
        public Optional<string> Code { get; }

        public Optional<Snowflake> ChannelId { get; }

        public Optional<Snowflake> InviterId { get; }

        public Optional<int> MaxUses { get; }

        public Optional<int> Uses { get; }

        public Optional<bool> IsTemporary { get; }

        public Optional<TimeSpan> MaxAge { get; }

        public TransientInviteAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
        {
            var changes = new TransientInviteAuditLogChanges(client, model);
            if (isCreated)
            {
                Code = changes.Code.NewValue;
                ChannelId = changes.ChannelId.NewValue;
                InviterId = changes.InviterId.NewValue;
                MaxUses = changes.MaxUses.NewValue;
                Uses = changes.Uses.NewValue;
                IsTemporary = changes.IsTemporary.NewValue;
                MaxAge = changes.MaxAge.NewValue;
            }
            else
            {
                Code = changes.Code.OldValue;
                ChannelId = changes.ChannelId.OldValue;
                InviterId = changes.InviterId.OldValue;
                MaxUses = changes.MaxUses.OldValue;
                Uses = changes.Uses.OldValue;
                IsTemporary = changes.IsTemporary.OldValue;
                MaxAge = changes.MaxAge.OldValue;
            }
        }
    }
}
