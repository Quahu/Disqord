using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a slash command option.
    /// </summary>
    public interface ISlashCommandOption : INamable, IJsonUpdatable<ApplicationCommandOptionJsonModel>
    {
        /// <summary>
        ///     Gets the type of this slash command option.
        /// </summary>
        SlashCommandOptionType Type { get; }

        /// <summary>
        ///     Gets the description of this slash command option.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets whether this slash command option is required.
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        ///     Gets the choices of this slash command option.
        /// </summary>
        IReadOnlyList<ISlashCommandOptionChoice> Choices { get; }

        /// <summary>
        ///     Gets the nested options of this slash command option.
        /// </summary>
        IReadOnlyList<ISlashCommandOption> Options { get; }

        /// <summary>
        ///     Gets the restricted channel types of this slash command option.
        /// </summary>
        IReadOnlyList<ChannelType> ChannelTypes { get; }
    }
}
