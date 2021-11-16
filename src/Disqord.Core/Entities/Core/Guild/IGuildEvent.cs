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
        ///     Gets the ID of the user who created this guild event.
        /// </summary>
        Snowflake? CreatorId { get; }

        /// <summary>
        ///     Gets the description of this guild event.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets the image hash of this guild event.
        /// </summary>
        string ImageHash { get; }

        /// <summary>
        ///     Gets the time this guild event will start.
        /// </summary>
        DateTimeOffset StartTime { get; }

        /// <summary>
        ///     Gets the time this guild event will end.
        ///     Returns <see langword="null"/> if the event has no scheduled end time.
        /// </summary>
        DateTimeOffset? EndTime { get; }

        /// <summary>
        ///     Gets the privacy level of this guild event.
        /// </summary>
        PrivacyLevel PrivacyLevel { get; }

        /// <summary>
        ///     Gets the status of this guild event.
        /// </summary>
        GuildEventStatus Status { get; }

        /// <summary>
        ///     Gets the target entity type of this guild event.
        /// </summary>
        GuildEventTargetType TargetEntityType { get; }

        /// <summary>
        ///     Gets the ID of the entity of this guild event.
        /// </summary>
        Snowflake? EntityId { get; }

        /// <summary>
        ///     Gets the entity metadata of this guild event.
        ///     Returns <see langword="null"/> if the event has no entity metadata.
        /// </summary>
        IGuildEventMetadata Metadata { get; }

        /// <summary>
        ///     Gets the user who created this guild event.
        /// </summary>
        IUser Creator { get; }

        /// <summary>
        ///     Gets the amount of members subscribed to this guild event.
        /// </summary>
        int? MemberCount { get; }
    }
}
