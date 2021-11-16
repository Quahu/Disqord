using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public interface IGuildEventMetadata : IEntity, IJsonUpdatable<GuildScheduledEventEntityMetadataJsonModel>
    {
        /// <summary>
        ///     Gets the IDs of the speakers in the event.
        /// </summary>
        IReadOnlyList<Snowflake> SpeakerIds { get; }

        /// <summary>
        ///     Gets the location of the event.
        /// </summary>
        string Location { get; }
    }
}
