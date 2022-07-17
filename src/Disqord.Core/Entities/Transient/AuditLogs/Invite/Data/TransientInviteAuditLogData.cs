using System;
using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs;

public class TransientInviteAuditLogData : IInviteAuditLogData
{
    /// <inheritdoc/>
    public Optional<string> Code { get; }

    /// <inheritdoc/>
    public Optional<Snowflake> ChannelId { get; }

    /// <inheritdoc/>
    public Optional<Snowflake> InviterId { get; }

    /// <inheritdoc/>
    public Optional<int> MaxUses { get; }

    /// <inheritdoc/>
    public Optional<int> Uses { get; }

    /// <inheritdoc/>
    public Optional<bool> IsTemporary { get; }

    /// <inheritdoc/>
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
