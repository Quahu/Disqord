using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a slash command option.
    /// </summary>
    public interface ISlashCommandOption : IEntity, INamableEntity, IJsonUpdatable<ApplicationCommandOptionJsonModel>
    {
        /// <summary>
        ///     Gets the type of this option.
        /// </summary>
        SlashCommandOptionType Type { get; }

        /// <summary>
        ///     Gets the description of this option.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets whether this option is required.
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        ///     Gets the choices of this option.
        /// </summary>
        IReadOnlyList<ISlashCommandOptionChoice> Choices { get; }

        /// <summary>
        ///     Gets whether this option supports auto-complete.
        /// </summary>
        bool HasAutoComplete { get; }

        /// <summary>
        ///     Gets the nested options of this option.
        /// </summary>
        IReadOnlyList<ISlashCommandOption> Options { get; }

        /// <summary>
        ///     Gets the channel types this option is restricted to.
        /// </summary>
        IReadOnlyList<ChannelType> ChannelTypes { get; }
    }
}
