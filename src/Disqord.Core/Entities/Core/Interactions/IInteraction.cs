using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an interaction
    /// </summary>
    public interface IInteraction : ISnowflakeEntity, IPossibleGuildEntity, IChannelEntity, IJsonUpdatable<InteractionJsonModel>
    {
        /// <summary>
        ///     Gets the ID of the application of this interaction.
        /// </summary>
        Snowflake ApplicationId { get; }

        /// <summary>
        ///     Gets the version of this interaction.
        /// </summary>
        int Version { get; }

        /// <summary>
        ///     Gets the type of this interaction.
        /// </summary>
        InteractionType Type { get; }

        /// <summary>
        ///     Gets the token of this interaction.
        /// </summary>
        string Token { get; }

        /// <summary>
        ///     Gets the user/member who triggered this interaction.
        /// </summary>
        IUser Author { get; }
    }
}
