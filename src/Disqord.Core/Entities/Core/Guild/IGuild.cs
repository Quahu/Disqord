using System;
using System.Collections.Generic;
using System.Globalization;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a guild.
/// </summary>
public interface IGuild : ISnowflakeEntity, INamableEntity, IJsonUpdatable<GuildJsonModel>
{
    /// <summary>
    ///     Gets the icon image hash of this guild.
    /// </summary>
    string? IconHash { get; }

    /// <summary>
    ///     Gets the splash image hash of this guild.
    /// </summary>
    string? SplashHash { get; }

    /// <summary>
    ///     Gets the discovery image hash of this guild.
    /// </summary>
    string? DiscoverySplashHash { get; }

    /// <summary>
    ///     Gets the owner ID of this guild.
    /// </summary>
    Snowflake OwnerId { get; }

    /// <summary>
    ///     Gets the AFK channel ID of this guild.
    /// </summary>
    Snowflake? AfkChannelId { get; }

    /// <summary>
    ///     Gets the AFK timeout of this guild.
    /// </summary>
    TimeSpan AfkTimeout { get; }

    /// <summary>
    ///     Gets whether this guild has the widget enabled.
    /// </summary>
    bool IsWidgetEnabled { get; }

    /// <summary>
    ///     Gets the widget channel ID of this guild.
    /// </summary>
    Snowflake? WidgetChannelId { get; }

    /// <summary>
    ///     Gets the <see cref="GuildVerificationLevel"/> of this guild.
    /// </summary>
    GuildVerificationLevel VerificationLevel { get; }

    /// <summary>
    ///     Gets the <see cref="GuildNotificationLevel"/> of this guild.
    /// </summary>
    GuildNotificationLevel NotificationLevel { get; }

    /// <summary>
    ///     Gets the <see cref="GuildContentFilterLevel"/> of this guild.
    /// </summary>
    GuildContentFilterLevel ContentFilterLevel { get; }

    /// <summary>
    ///     Gets the roles of this guild.
    /// </summary>
    IReadOnlyDictionary<Snowflake, IRole> Roles { get; }

    /// <summary>
    ///     Gets the emojis of this guild.
    /// </summary>
    IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis { get; }

    /// <summary>
    ///     Gets the features of this guild.
    /// </summary>
    /// <remarks>
    ///     For most use cases, <see cref="GuildExtensions.GetFeatures(IGuild)"/> should be preferred for simplicity.
    /// </remarks>
    IReadOnlyList<string> Features { get; }

    /// <summary>
    ///     Gets the <see cref="GuildMfaLevel"/> of this guild.
    /// </summary>
    GuildMfaLevel MfaLevel { get; }

    /// <summary>
    ///     Gets the application ID of this guild.
    ///     Returns a valid value for, for example, guilds created by bots.
    /// </summary>
    Snowflake? ApplicationId { get; }

    /// <summary>
    ///     Gets the system channel ID of this guild.
    /// </summary>
    Snowflake? SystemChannelId { get; }

    /// <summary>
    ///     Gets the system channel flags of this guild.
    /// </summary>
    SystemChannelFlags SystemChannelFlags { get; }

    /// <summary>
    ///     Gets the rules channel ID of this guild.
    /// </summary>
    Snowflake? RulesChannelId { get; }

    /// <summary>
    ///     Gets the max presence count of this guild.
    /// </summary>
    int? MaxPresenceCount { get; }

    /// <summary>
    ///     Gets the max member count of this guild.
    /// </summary>
    int? MaxMemberCount { get; }

    /// <summary>
    ///     Gets the vanity URL code of this guild.
    /// </summary>
    string? VanityUrlCode { get; }

    /// <summary>
    ///     Gets the description of this guild.
    /// </summary>
    string? Description { get; }

    /// <summary>
    ///     Gets the banner image hash of this guild.
    /// </summary>
    string? BannerHash { get; }

    /// <summary>
    ///     Gets the <see cref="Disqord.GuildBoostTier"/> of this guild.
    /// </summary>
    GuildBoostTier BoostTier { get; }

    /// <summary>
    ///     Gets the amount of boosting members of this guild.
    /// </summary>
    int? BoostingMemberCount { get; }

    /// <summary>
    ///     Gets the preferred locale of this guild.
    /// </summary>
    CultureInfo PreferredLocale { get; }

    /// <summary>
    ///     Gets the public updates channel ID of this guild.
    /// </summary>
    Snowflake? PublicUpdatesChannelId { get; }

    /// <summary>
    ///     Gets the max video member count of this guild.
    /// </summary>
    int? MaxVideoMemberCount { get; }

    /// <summary>
    ///     Gets the NSFW level of this guild.
    /// </summary>
    GuildNsfwLevel NsfwLevel { get; }

    /// <summary>
    ///     Gets the stickers of this guild.
    /// </summary>
    IReadOnlyDictionary<Snowflake, IGuildSticker> Stickers { get; }

    /// <summary>
    ///     Gets whether this guild has the boost progress bar enabled.
    /// </summary>
    bool IsBoostProgressBarEnabled { get; }
}
