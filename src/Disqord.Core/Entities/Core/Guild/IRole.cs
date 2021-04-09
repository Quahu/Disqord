using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a guild role.
    /// </summary>
    public interface IRole : ISnowflakeEntity, IGuildEntity, INamable, IMentionable, IJsonUpdatable<RoleJsonModel>
    {
        /// <summary>
        ///     Gets the color of this role.
        ///     Returns <see langword="null"/> if the role has the default color.
        /// </summary>
        Color? Color { get; }

        /// <summary>
        ///     Gets whether this role is hoisted.
        /// </summary>
        bool IsHoisted { get; }

        /// <summary>
        ///     Gets the position of this role.
        /// </summary>
        int Position { get; }

        /// <summary>
        ///     Gets the permissions of this role.
        /// </summary>
        GuildPermissions Permissions { get; }

        /// <summary>
        ///     Gets whether this role is managed by an integration.
        /// </summary>
        bool IsManaged { get; }

        /// <summary>
        ///     Gets whether this role is mentionable.
        /// </summary>
        bool IsMentionable { get; }
    }
}
