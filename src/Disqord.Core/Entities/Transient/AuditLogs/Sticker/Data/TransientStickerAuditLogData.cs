using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs;

public class TransientStickerAuditLogData : IStickerAuditLogData
{
    /// <inheritdoc/>
    public Optional<string> Name { get; }

    /// <inheritdoc/>
    public Optional<string> Description { get; }

    /// <inheritdoc/>
    public Optional<string> Tags { get; }

    /// <inheritdoc/>
    public Optional<StickerFormatType> FormatType { get; }

    /// <inheritdoc/>
    public Optional<bool> IsAvailable { get; }

    /// <inheritdoc/>
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
