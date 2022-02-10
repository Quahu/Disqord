using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a slash command command auto-complete interaction.
    /// </summary>
    public interface ISlashCommandAutoCompleteInteraction : IApplicationCommandInteraction
    {
        /// <summary>
        ///     Gets the options of this slash command interaction.
        /// </summary>
        IReadOnlyDictionary<string, ISlashCommandAutoCompleteInteractionOption> Options { get; }
    }
}
