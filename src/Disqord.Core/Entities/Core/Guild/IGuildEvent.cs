using System;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a guild event.
    /// </summary>
    public interface IGuildEvent : ISnowflakeEntity, IGuildEntity, IPossibleChannelEntity, INamableEntity, IJsonUpdatable<GuildScheduledEventJsonModel>
    {
        /// <summary>
        ///     Gets the ID of the user who created this event.
        /// </summary>
        /// <remarks>
        ///     This property is only available for events created after October 25th, 2021.
        /// </remarks>
        /// <returns>
        ///     The ID of the user who created this event or <see langword="null"/> for older events (see remarks).
        /// </returns>
        Snowflake? CreatorId { get; }

        /// <summary>
        ///     Gets the user who created this event.
        /// </summary>
        /// <remarks>
        ///     This property is only available for events created after October 25th, 2021.
        /// </remarks>
        /// <returns>
        ///     The user who created this event or <see langword="null"/> for older events (see remarks).
        /// </returns>
        IUser Creator { get; }

        /// <summary>
        ///     Gets the description of this event.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets the cover image hash of this event.
        /// </summary>
        string CoverImageHash { get; }

        /// <summary>
        ///     Gets the time this event will start.
        /// </summary>
        DateTimeOffset StartsAt { get; }

        /// <summary>
        ///     Gets the time this event will end.
        /// </summary>
        /// <returns>
        ///     The time this event will end or <see langword="null"/> if the event has no scheduled end time.
        /// </returns>
        DateTimeOffset? EndsAt { get; }

        /// <summary>
        ///     Gets the privacy level of this event.
        /// </summary>
        PrivacyLevel PrivacyLevel { get; }

        /// <summary>
        ///     Gets the status of this event.
        /// </summary>
        GuildEventStatus Status { get; }

        /// <summary>
        ///     Gets the target type of this event.
        /// </summary>
        GuildEventTargetType TargetType { get; }

        /// <summary>
        ///     Gets the ID of the target of this event.
        /// </summary>
        /// <remarks>
        ///     This property is <see langword="null"/> when <see cref="IGuildEvent.TargetType"/> is <see cref="GuildEventTargetType.External"/>.
        /// </remarks>
        Snowflake? TargetId { get; }

        /// <summary>
        ///     Gets the entity metadata of this event.
        /// </summary>
        /// <returns>
        ///     The entity metadata of this event or <see langword="null"/> if the event has no entity metadata.
        /// </returns>
        IGuildEventMetadata Metadata { get; }

        /// <summary>
        ///     Gets the amount of members subscribed to this event.
        /// </summary>
        /// <remarks>
        ///     This property is only available when the event is fetched with the <c>withSubscriberCount</c> parameter set to <see langword="true" />.
        /// </remarks>
        int? SubscriberCount { get; }
    }
}
