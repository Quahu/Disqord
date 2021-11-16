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
        Snowflake? CreatorId { get; }

        /// <summary>
        ///     Gets the user who created this event.
        /// </summary>
        IUser Creator { get; }

        /// <summary>
        ///     Gets the description of this event.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets the image hash of this event.
        /// </summary>
        string ImageHash { get; }

        /// <summary>
        ///     Gets the time this event will start.
        /// </summary>
        DateTimeOffset StartsAt { get; }

        /// <summary>
        ///     Gets the time this event will end.
        ///     Returns <see langword="null"/> if the event has no scheduled end time.
        /// </summary>
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
        Snowflake? TargetId { get; }

        /// <summary>
        ///     Gets the entity metadata of this event.
        ///     Returns <see langword="null"/> if the event has no entity metadata.
        /// </summary>
        IGuildEventMetadata Metadata { get; }

        /// <summary>
        ///     Gets the amount of members subscribed to this event.
        /// </summary>
        int? MemberCount { get; }
    }
}
