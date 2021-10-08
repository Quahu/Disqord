using System;
using Disqord.Models;

namespace Disqord
{
    public interface IIntegration : ISnowflakeEntity, IGuildEntity, INamableEntity, IJsonUpdatable<IntegrationJsonModel>
    {
        /// <summary>
        ///     Gets the type of this integration.
        ///     E.g. <c>twitch</c>, <c>youtube</c>, or <c>discord</c>.
        /// </summary>
        string Type { get; }

        /// <summary>
        ///     Gets whether this integration is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        ///     Gets whether this integration is currently syncing.
        ///     Always returns <see langword="false"/> for bot integrations.
        /// </summary>
        bool IsSyncing { get; }

        /// <summary>
        ///     Gets the ID of the role of this integration.
        ///     Always returns <see langword="null"/> for bot integrations.
        /// </summary>
        Snowflake? RoleId { get; }

        /// <summary>
        ///     Gets whether this integration enables emojis.
        ///     This is used, for example, for the Twitch integration allowing the use of Twitch emojis in Discord.
        ///     Always returns <see langword="false"/> for bot integrations.
        /// </summary>
        bool EnablesEmojis { get; }

        /// <summary>
        ///     Gets the expiration behavior of this integration.
        ///     Always returns <see langword="null"/> for bot integrations.
        /// </summary>
        IntegrationExpirationBehavior? ExpirationBehavior { get; }

        /// <summary>
        ///     Gets the expiration grace period of this integration.
        ///     Always returns <see langword="null"/> for bot integrations.
        /// </summary>
        TimeSpan? ExpirationGracePeriod { get; }

        /// <summary>
        ///     Gets the user of this integration.
        ///     For bot integrations, this is the user who authorized the bot.
        ///     Returns <see langword="null"/> for old integrations.
        /// </summary>
        IUser User { get; }

        /// <summary>
        ///     Gets the account of this integration.
        /// </summary>
        IIntegrationAccount Account { get; }

        /// <summary>
        ///     Gets when this integration was synced at.
        ///     Always returns <see langword="null"/> for bot integrations.
        /// </summary>
        DateTimeOffset? SyncedAt { get; }

        /// <summary>
        ///     Gets the amount of subscribed users of this integration.
        ///     Always returns <see langword="null"/> for bot integrations.
        /// </summary>
        int? SubscriberCount { get; }

        /// <summary>
        ///     Gets whether this integration has been revoked.
        ///     Always returns <see langword="false"/> for bot integrations.
        /// </summary>
        bool IsRevoked { get; }

        /// <summary>
        ///     Gets the application of this integration.
        ///     Returns <see langword="null"/> for non-<c>discord</c> integrations.
        /// </summary>
        IIntegrationApplication Application { get; }
    }
}
