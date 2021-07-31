using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a guild's welcome screen.
    /// </summary>
    public interface IGuildWelcomeScreen
    {
        /// <summary>
        ///     Gets the description of this welcome screen.
        /// </summary>
        string Description { get; }

        IReadOnlyList<IGuildWelcomeScreenChannel> Channels { get; }
    }
}