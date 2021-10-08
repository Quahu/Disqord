using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an application command.
    /// </summary>
    public interface IApplicationCommand : ISnowflakeEntity, IPossibleGuildEntity, INamableEntity, IJsonUpdatable<ApplicationCommandJsonModel>
    {
        /// <summary>
        ///     Gets the type of this command.
        /// </summary>
        ApplicationCommandType Type { get; }

        /// <summary>
        ///     Gets the ID of the application of this command.
        /// </summary>
        Snowflake ApplicationId { get; }

        /// <summary>
        ///     Gets whether this application command is enabled by default.
        /// </summary>
        bool IsEnabledByDefault { get; }

        /// <summary>
        ///     Gets the auto-incrementing version of this command.
        /// </summary>
        /// <remarks>
        ///     The <see cref="Snowflake"/> is automatically incremented by Discord each time the command is updated.
        ///     Note that this seems to happen in bulk updates even if the command remains unchanged.
        /// </remarks>
        Snowflake Version { get; }
    }
}
