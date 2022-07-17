using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents a guild's welcome screen.
/// </summary>
public interface IGuildWelcomeScreen : IGuildEntity
{
    /// <summary>
    ///     Gets the description of this welcome screen.
    /// </summary>
    string? Description { get; }

    /// <summary>
    ///     Gets the channels of this welcome screen.
    /// </summary>
    IReadOnlyList<IGuildWelcomeScreenChannel> Channels { get; }
}
