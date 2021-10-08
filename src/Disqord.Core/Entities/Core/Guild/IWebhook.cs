using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a webhook.
    /// </summary>
    public interface IWebhook : ISnowflakeEntity, IChannelEntity, IGuildEntity, INamableEntity, IJsonUpdatable<WebhookJsonModel>
    {
        /// <summary>
        ///     Gets the avatar image hash of this webhook.
        /// </summary>
        string AvatarHash { get; }

        /// <summary>
        ///     Gets the creator of this webhook.
        ///     Returns <see langword="null"/> for webhooks fetched with no authorization.
        /// </summary>
        IUser Creator { get; }

        /// <summary>
        ///     Gets the token of this webhook.
        /// </summary>
        string Token { get; }

        /// <summary>
        ///     Gets the <see cref="WebhookType"/> of this webhook.
        /// </summary>
        WebhookType Type { get; }
    }
}
