using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a guild preview.
    /// </summary>
    public interface IGuildPreview : IGuildEntity, INamable, ISnowflakeEntity
    {
        /// <summary>
        ///     Gets the icon image hash of this guild.
        /// </summary>
        string IconHash { get; }
        
        /// <summary>
        ///     Gets the splash image hash of this guild.
        /// </summary>
        string SplashHash { get; }
        
        /// <summary>
        ///     Gets the discovery image hash of this guild.
        /// </summary>
        string DiscoverySplashHash { get; }
        
        /// <summary>
        ///     Gets the features of this guild.
        /// </summary>
        GuildFeatures Features { get; }
        
        /// <summary>
        ///     Gets the emojis of the guild.
        /// </summary>
        IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis { get; }
        
        /// <summary>
        ///  Gets the approximate number of members in the guild.
        /// </summary>
        int ApproximateMemberCount { get; }
        
        /// <summary>
        ///     Gets the approximate number of presences in this guild.
        /// </summary>
        int ApproximatePresenceCount { get; }
        
        /// <summary>
        ///     Gets the description for the guild.
        ///     Only present if the guild is discoverable.
        /// </summary>
        string Description { get; }
    }
}