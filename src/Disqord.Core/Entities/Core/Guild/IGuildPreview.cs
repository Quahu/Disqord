using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a guild preview.
/// </summary>
public interface IGuildPreview : IGuildEntity, INamableEntity, IJsonUpdatable<GuildPreviewJsonModel>
{
    /// <summary>
    ///     Gets the icon image hash of the guild.
    /// </summary>
    string? IconHash { get; }

    /// <summary>
    ///     Gets the splash image hash of the guild.
    /// </summary>
    string? SplashHash { get; }

    /// <summary>
    ///     Gets the discovery image hash of the guild.
    /// </summary>
    string? DiscoverySplashHash { get; }

    /// <summary>
    ///     Gets the features of the guild.
    /// </summary>
    IReadOnlyList<string> Features { get; }

    /// <summary>
    ///     Gets the emojis of the guild.
    /// </summary>
    IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis { get; }

    /// <summary>
    ///  Gets the approximate number of members in the guild.
    /// </summary>
    int ApproximateMemberCount { get; }

    /// <summary>
    ///     Gets the approximate number of presences in the guild.
    /// </summary>
    int ApproximatePresenceCount { get; }

    /// <summary>
    ///     Gets the description of the guild.
    ///     Only present if the guild is discoverable.
    /// </summary>
    string? Description { get; }
}
