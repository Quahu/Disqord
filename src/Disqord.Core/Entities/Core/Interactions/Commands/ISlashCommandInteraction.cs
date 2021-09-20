using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a text application command interaction, i.e. a slash command.
    /// </summary>
    public interface ISlashCommandInteraction : IApplicationCommandInteraction
    {
        /// <summary>
        ///     Gets the options of this text interaction.
        /// </summary>
        IReadOnlyDictionary<string, ISlashCommandInteractionOption> Options { get; }
    }
}
