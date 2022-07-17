using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs;

public class TransientGuildEventAuditLogData : IGuildEventAuditLogData
{
    /// <inheritdoc/>
    public Optional<Snowflake> ChannelId { get; }

    /// <inheritdoc/>
    public Optional<string> Name { get; }

    /// <inheritdoc/>
    public Optional<string?> Description { get; }

    /// <inheritdoc/>
    public Optional<string?> CoverImageHash { get; }

    /// <inheritdoc/>
    public Optional<GuildEventTargetType> TargetType { get; }

    /// <inheritdoc/>
    public Optional<string?> Location { get; }

    /// <inheritdoc/>
    public Optional<PrivacyLevel> PrivacyLevel { get; }

    /// <inheritdoc/>
    public Optional<GuildEventStatus> Status { get; }

    public TransientGuildEventAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
    {
        var changes = new TransientGuildEventAuditLogChanges(client, model);
        if (isCreated)
        {
            ChannelId = changes.ChannelId.NewValue;
            Name = changes.Name.NewValue;
            Description = changes.Description.NewValue;
            CoverImageHash = changes.CoverImageHash.NewValue;
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
            CoverImageHash = changes.CoverImageHash.OldValue;
            TargetType = changes.TargetType.OldValue;
            Location = changes.Location.OldValue;
            PrivacyLevel = changes.PrivacyLevel.OldValue;
            Status = changes.Status.OldValue;
        }
    }
}
