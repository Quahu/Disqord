using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an application command's guild permission
    /// </summary>
    public interface IApplicationCommandGuildPermissions : ISnowflakeEntity, IGuildEntity, IJsonUpdatable<ApplicationCommandGuildPermissionsJsonModel>
    {
        /// <summary>
        ///     Gets the ID of the application of the command
        /// </summary>
        Snowflake ApplicationId { get; }

        /// <summary>
        ///     Gets the permissions of the command
        /// </summary>
        IReadOnlyList<IApplicationCommandPermissions> Permissions { get; }
    }
}