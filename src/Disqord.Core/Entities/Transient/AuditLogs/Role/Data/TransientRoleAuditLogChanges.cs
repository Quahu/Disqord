using Disqord.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.AuditLogs;

public class TransientRoleAuditLogChanges : IRoleAuditLogChanges
{
    /// <inheritdoc/>
    public AuditLogChange<string> Name { get; }

    /// <inheritdoc/>
    public AuditLogChange<Permissions> Permissions { get; }

    /// <inheritdoc/>
    public AuditLogChange<Color?> Color { get; }

    /// <inheritdoc/>
    public AuditLogChange<bool> IsHoisted { get; }

    /// <inheritdoc/>
    public AuditLogChange<string> IconHash { get; }

    /// <inheritdoc/>
    public AuditLogChange<bool> IsMentionable { get; }

    /// <inheritdoc/>
    public AuditLogChange<string> UnicodeEmoji { get; }

    public TransientRoleAuditLogChanges(IClient client, AuditLogEntryJsonModel model)
    {
        for (var i = 0; i < model.Changes.Value.Length; i++)
        {
            var change = model.Changes.Value[i];
            switch (change.Key)
            {
                case "name":
                {
                    Name = AuditLogChange<string>.Convert(change);
                    break;
                }
                case "permissions":
                {
                    Permissions = AuditLogChange<Permissions>.Convert(change);
                    break;
                }
                case "color":
                {
                    Color = AuditLogChange<Color?>.Convert<int>(change, x => x != 0 ? x : null);
                    break;
                }
                case "hoist":
                {
                    IsHoisted = AuditLogChange<bool>.Convert(change);
                    break;
                }
                case "icon_hash":
                {
                    IconHash = AuditLogChange<string>.Convert(change);
                    break;
                }
                case "mentionable":
                {
                    IsMentionable = AuditLogChange<bool>.Convert(change);
                    break;
                }
                case "unicode_emoji":
                {
                    UnicodeEmoji = AuditLogChange<string>.Convert(change);
                    break;
                }
                default:
                {
                    client.Logger.LogDebug("Unknown key {0} for {1}", change.Key, this);
                    break;
                }
            }
        }
    }
}
