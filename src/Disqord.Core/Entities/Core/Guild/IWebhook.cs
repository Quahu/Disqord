using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a webhook.
/// </summary>
public interface IWebhook : ISnowflakeEntity, IPossiblyChannelEntity, IPossiblyGuildEntity, IPossiblyNamableEntity, IJsonUpdatable<WebhookJsonModel>
{
    /// <summary>
    ///     Gets the avatar image hash of this webhook.
    /// </summary>
    string? AvatarHash { get; }

    /// <summary>
    ///     Gets the creator of this webhook.
    /// </summary>
    /// <returns>
    ///     The creator of this webhook or <see langword="null"/> for webhooks fetched with no authorization.
    /// </returns>
    IUser? Creator { get; }

    /// <summary>
    ///     Gets the token of this webhook.
    /// </summary>
    string? Token { get; }

    /// <summary>
    ///     Gets the <see cref="WebhookType"/> of this webhook.
    /// </summary>
    WebhookType Type { get; }
}
