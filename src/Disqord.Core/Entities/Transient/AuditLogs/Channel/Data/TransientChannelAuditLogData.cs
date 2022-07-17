using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs;

public class TransientChannelAuditLogData : IChannelAuditLogData
{
    /// <inheritdoc/>
    public Optional<string> Name { get; }

    /// <inheritdoc/>
    public Optional<string?> Topic { get; }

    /// <inheritdoc/>
    public Optional<int> Bitrate { get; }

    /// <inheritdoc/>
    public Optional<int> MemberLimit { get; }

    /// <inheritdoc/>
    public Optional<IReadOnlyList<IOverwrite>> Overwrites { get; }

    /// <inheritdoc/>
    public Optional<bool> IsAgeRestricted { get; }

    /// <inheritdoc/>
    public Optional<TimeSpan> Slowmode { get; }

    /// <inheritdoc/>
    public Optional<ChannelType> Type { get; }

    /// <inheritdoc/>
    public Optional<string?> Region { get; }

    public TransientChannelAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
    {
        var changes = new TransientChannelAuditLogChanges(client, model);
        if (isCreated)
        {
            Name = changes.Name.NewValue;
            Topic = changes.Topic.NewValue;
            Bitrate = changes.Bitrate.NewValue;
            MemberLimit = changes.MemberLimit.NewValue;
            Overwrites = changes.Overwrites.NewValue;
            IsAgeRestricted = changes.IsAgeRestricted.NewValue;
            Slowmode = changes.Slowmode.NewValue;
            Type = changes.Type.NewValue;
            Region = changes.Region.NewValue;
        }
        else
        {
            Name = changes.Name.OldValue;
            Topic = changes.Topic.OldValue;
            Bitrate = changes.Bitrate.OldValue;
            MemberLimit = changes.MemberLimit.OldValue;
            Overwrites = changes.Overwrites.OldValue;
            IsAgeRestricted = changes.IsAgeRestricted.OldValue;
            Slowmode = changes.Slowmode.OldValue;
            Type = changes.Type.OldValue;
            Region = changes.Region.OldValue;
        }
    }
}
