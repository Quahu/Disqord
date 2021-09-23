using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an application command.
    /// </summary>
    public interface IApplicationCommand : ISnowflakeEntity, IPossibleGuildEntity, INamable, IJsonUpdatable<ApplicationCommandJsonModel>
    {
        /// <summary>
        ///     Gets the type of this application command.
        /// </summary>
        ApplicationCommandType Type { get; }

        /// <summary>
        ///     Gets the ID of the application of this application command.
        /// </summary>
        Snowflake ApplicationId { get; }

        /// <summary>
        ///     Gets whether this application command is enabled by default.
        /// </summary>
        bool IsEnabledByDefault { get; }

        /// <summary>
        ///     Gets the auto-incrementing version of this application command.
        /// </summary>
        Snowflake Version { get; }
    }
}
