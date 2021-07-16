using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an application command
    /// </summary>
    public interface IApplicationCommand : ISnowflakeEntity, INamable, IJsonUpdatable<ApplicationJsonModel>
    {
        /// <summary>
        ///     Gets the ID of the application of this command
        /// </summary>
        Snowflake ApplicationId { get; }

        /// <summary>
        ///     Gets the description of this command
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets the options of this command
        /// </summary>
        IReadOnlyList<IApplicationCommandOption> Options { get; }

        /// <summary>
        ///     Gets whether this command has default permission
        /// </summary>
        bool HasDefaultPermission { get; }
    }
}