using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs;

public class TransientWebhookAuditLogData : IWebhookAuditLogData
{
    /// <inheritdoc/>
    public Optional<string> Name { get; }

    /// <inheritdoc/>
    public Optional<WebhookType> Type { get; }

    /// <inheritdoc/>
    public Optional<string?> AvatarHash { get; }

    /// <inheritdoc/>
    public Optional<Snowflake> ChannelId { get; }

    /// <inheritdoc/>
    public Optional<Snowflake?> ApplicationId { get; }

    public TransientWebhookAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
    {
        var changes = new TransientWebhookAuditLogChanges(client, model);
        if (isCreated)
        {
            Name = changes.Name.NewValue;
            Type = changes.Type.NewValue;
            AvatarHash = changes.AvatarHash.NewValue;
            ChannelId = changes.ChannelId.NewValue;
            ApplicationId = changes.ApplicationId.NewValue;
        }
        else
        {
            Name = changes.Name.OldValue;
            Type = changes.Type.OldValue;
            AvatarHash = changes.AvatarHash.OldValue;
            ChannelId = changes.ChannelId.OldValue;
            ApplicationId = changes.ApplicationId.OldValue;
        }
    }
}
