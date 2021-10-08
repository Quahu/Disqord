using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a guild role.
    /// </summary>
    public interface IRole : ISnowflakeEntity, IGuildEntity, INamableEntity, IMentionableEntity, IJsonUpdatable<RoleJsonModel>
    {
        /// <summary>
        ///     Gets the color of this role.
        ///     Returns <see langword="null"/> if the role has the default color.
        /// </summary>
        Color? Color { get; }

        /// <summary>
        ///     Gets whether this role is hoisted, i.e. whether members of this role
        ///     are displayed separately on the member list in the client.
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

        /// <summary>
        ///     Gets the role tags of this role.
        ///     This can be used to, for example, determine if the role is the Nitro booster role.
        ///     <example>
        ///     Finding the Nitro booster role.
        ///     <code>
        ///     var roles = guild.GetRoles().Values;
        ///     var boosterRole = roles.FirstOrDefault(x => x.Tags.IsNitroBooster);
        ///     </code>
        ///     </example>
        /// </summary>
        IRoleTags Tags { get; }
    }
}
