using System;
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a guild event.
    /// </summary>
    public interface IGuildEvent : ISnowflakeEntity, IGuildEntity, IPossibleChannelEntity, INamable, IJsonUpdatable<GuildEventJsonModel>
    {
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
        StagePrivacyLevel PrivacyLevel { get; }

        /// <summary>
        ///     Gets the event status of this guild event.
        /// </summary>
        GuildEventStatus Status { get; }

        /// <summary>
        ///     Gets the entity type of this guild event.
        /// </summary>
        GuildEventTarget EntityType { get; }

        /// <summary>
        ///     Gets the ID of the entity of this guild event.
        /// </summary>
        Snowflake? EntityId { get; }

        /// <summary>
        ///     Gets the IDs of the "Game SKUs" of this guild event.
        /// </summary>
        IReadOnlyList<Snowflake> SkuIds { get; }

        /// <summary>
        ///     Gets the amount of users subscribed to this guild event.
        /// </summary>
        int? UserCount { get; }

        /// <summary>
        ///     Gets the IDs of the speakers in the event.
        /// </summary>
        IReadOnlyList<Snowflake> SpeakerIds { get; }
    }
}
