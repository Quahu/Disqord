using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an application command permission
    /// </summary>
    public interface IApplicationCommandPermissions : ISnowflakeEntity,
        IJsonUpdatable<ApplicationCommandPermissionsJsonModel>
    {
        /// <summary>
        ///     Gets the type of this permission
        /// </summary>
        ApplicationCommandPermissionType Type { get; }

        /// <summary>
        ///     Gets whether this permission is allowed
        /// </summary>
        bool IsAllowed { get; }
    }
}