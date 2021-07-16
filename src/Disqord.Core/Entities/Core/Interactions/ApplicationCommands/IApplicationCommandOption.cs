using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an application command option
    /// </summary>
    public interface IApplicationCommandOption : INamable, IJsonUpdatable<ApplicationCommandOptionJsonModel>
    {
        /// <summary>
        ///     Gets the type of this option
        /// </summary>
        ApplicationCommandOptionType Type { get; }

        /// <summary>
        ///     Gets the description of this option
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets whether this option is required
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        ///     Gets the choices of this option
        /// </summary>
        IReadOnlyList<IApplicationCommandOptionChoice> Choices { get; }

        /// <summary>
        ///     Gets the nested options of this option
        /// </summary>
        IReadOnlyList<IApplicationCommandOption> Options { get; }
    }
}