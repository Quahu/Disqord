using System;
using Disqord.Models;

namespace Disqord;

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
    /// </summary>
    /// <returns>
    ///     Always <see langword="false"/> for bot integrations.
    /// </returns>
    bool IsSyncing { get; }

    /// <summary>
    ///     Gets the ID of the role of this integration.
    /// </summary>
    /// <returns>
    ///     Always <see langword="null"/> for bot integrations.
    /// </returns>
    Snowflake? RoleId { get; }

    /// <summary>
    ///     Gets whether this integration enables emojis.
    ///     This is used, for example, for the Twitch integration allowing the use of Twitch emojis in Discord.
    /// </summary>
    /// <returns>
    ///     Always <see langword="false"/> for bot integrations.
    /// </returns>
    bool EnablesEmojis { get; }

    /// <summary>
    ///     Gets the expiration behavior of this integration.
    /// </summary>
    /// <returns>
    ///     Always <see langword="null"/> for bot integrations.
    /// </returns>
    IntegrationExpirationBehavior? ExpirationBehavior { get; }

    /// <summary>
    ///     Gets the expiration grace period of this integration.
    /// </summary>
    /// <returns>
    ///     Always <see langword="null"/> for bot integrations.
    /// </returns>
    TimeSpan? ExpirationGracePeriod { get; }

    /// <summary>
    ///     Gets the user of this integration.
    ///     For bot integrations, this is the user who authorized the bot.
    /// </summary>
    /// <returns>
    ///     Returns <see langword="null"/> for old integrations.
    /// </returns>
    IUser? User { get; }

    /// <summary>
    ///     Gets the account of this integration.
    /// </summary>
    IIntegrationAccount Account { get; }

    /// <summary>
    ///     Gets when this integration was synced at.
    /// </summary>
    /// <returns>
    ///     Always <see langword="null"/> for bot integrations.
    /// </returns>
    DateTimeOffset? SyncedAt { get; }

    /// <summary>
    ///     Gets the amount of subscribed users of this integration.
    /// </summary>
    /// <returns>
    ///     Always <see langword="null"/> for bot integrations.
    /// </returns>
    int? SubscriberCount { get; }

    /// <summary>
    ///     Gets whether this integration has been revoked.
    /// </summary>
    /// <returns>
    ///     Always <see langword="false"/> for bot integrations.
    /// </returns>
    bool IsRevoked { get; }

    /// <summary>
    ///     Gets the application of this integration.
    /// </summary>
    /// <returns>
    ///     Always <see langword="null"/> for non-<c>discord</c> integrations.
    /// </returns>
    IIntegrationApplication? Application { get; }
}
