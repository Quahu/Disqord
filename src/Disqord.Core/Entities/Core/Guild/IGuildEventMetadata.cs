using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a guild event's metadata.
/// </summary>
public interface IGuildEventMetadata : IEntity, IJsonUpdatable<GuildScheduledEventEntityMetadataJsonModel>
{
    /// <summary>
    ///     Gets the location of the event.
    /// </summary>
    string? Location { get; }
}
