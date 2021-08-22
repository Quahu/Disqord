using System.Collections.Generic;

namespace Disqord
{
    public interface IGuildEventMetadata
    {
        /// <summary>
        ///     Gets the IDs of the speakers in the event.
        /// </summary>
        IReadOnlyList<Snowflake> SpeakerIds { get; }
    }
}