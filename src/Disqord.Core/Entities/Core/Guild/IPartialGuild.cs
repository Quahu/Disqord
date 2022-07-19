using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

public interface IPartialGuild : ISnowflakeEntity, INamableEntity, IJsonUpdatable<GuildJsonModel>
{
    /// <summary>
    ///     Gets the icon image hash of this guild.
    /// </summary>
    string? IconHash { get; }

    /// <summary>
    ///     Gets whether the current user owns this guild.
    /// </summary>
    bool IsOwner { get; }

    /// <summary>
    ///     Gets the permissions of the current user in this guild.
    /// </summary>
    Permissions Permissions { get; }

    /// <summary>
    ///     Gets the features of this guild.
    /// </summary>
    IReadOnlyList<string> Features { get; }
}
