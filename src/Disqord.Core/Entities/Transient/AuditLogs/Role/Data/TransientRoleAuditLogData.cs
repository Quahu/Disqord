using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs;

public class TransientRoleAuditLogData : IRoleAuditLogData
{
    /// <inheritdoc/>
    public Optional<string> Name { get; }

    /// <inheritdoc/>
    public Optional<Permissions> Permissions { get; }

    /// <inheritdoc/>
    public Optional<Color?> Color { get; }

    /// <inheritdoc/>
    public Optional<bool> IsHoisted { get; }

    /// <inheritdoc/>
    public Optional<string> IconHash { get; }

    /// <inheritdoc/>
    public Optional<bool> IsMentionable { get; }

    /// <inheritdoc/>
    public Optional<string> UnicodeEmoji { get; }

    public TransientRoleAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
    {
        var changes = new TransientRoleAuditLogChanges(client, model);
        if (isCreated)
        {
            Name = changes.Name.NewValue;
            Permissions = changes.Permissions.NewValue;
            Color = changes.Color.NewValue;
            IsHoisted = changes.IsHoisted.NewValue;
            IconHash = changes.IconHash.NewValue;
            IsMentionable = changes.IsMentionable.NewValue;
            UnicodeEmoji = changes.UnicodeEmoji.NewValue;
        }
        else
        {
            Name = changes.Name.OldValue;
            Permissions = changes.Permissions.OldValue;
            Color = changes.Color.OldValue;
            IsHoisted = changes.IsHoisted.OldValue;
            IconHash = changes.IconHash.OldValue;
            IsMentionable = changes.IsMentionable.OldValue;
            UnicodeEmoji = changes.UnicodeEmoji.OldValue;
        }
    }
}
