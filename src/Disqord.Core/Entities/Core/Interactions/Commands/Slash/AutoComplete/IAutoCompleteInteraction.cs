using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents a slash command command auto-complete interaction.
/// </summary>
public interface IAutoCompleteInteraction : IApplicationCommandInteraction
{
    /// <summary>
    ///     Gets the options of this slash command interaction.
    /// </summary>
    IReadOnlyDictionary<string, ISlashCommandInteractionOption> Options { get; }

    /// <summary>
    ///     Gets the option currently focused by the user of this interaction.
    /// </summary>
    ISlashCommandInteractionOption FocusedOption { get; }
}