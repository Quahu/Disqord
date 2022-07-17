using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents a slash command command interaction.
/// </summary>
public interface ISlashCommandInteraction : IApplicationCommandInteraction
{
    /// <summary>
    ///     Gets the options of this slash command interaction.
    /// </summary>
    IReadOnlyDictionary<string, ISlashCommandInteractionOption> Options { get; }
}