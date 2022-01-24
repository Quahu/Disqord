using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public interface IGuildEventMetadata : IEntity, IJsonUpdatable<GuildScheduledEventEntityMetadataJsonModel>
    {
        /// <summary>
        ///     Gets the location of the event.
        /// </summary>
        string Location { get; }
    }
}
