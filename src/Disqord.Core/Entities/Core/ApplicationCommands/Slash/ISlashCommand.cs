using System.Collections.Generic;
using System.Globalization;

namespace Disqord;

/// <summary>
///     Represents a slash command.
/// </summary>
public interface ISlashCommand : IApplicationCommand
{
    /// <summary>
    ///     Gets the description of this slash command.
    /// </summary>
    string Description { get; }

    /// <summary>
    ///     Gets the description localizations of this command.
    /// </summary>
    /// <remarks>
    ///     Might be empty if retrieved using bulk application command fetch endpoints.
    /// </remarks>
    IReadOnlyDictionary<CultureInfo, string> DescriptionLocalizations { get; }

    /// <summary>
    ///     Gets the options of this slash command.
    /// </summary>
    IReadOnlyList<ISlashCommandOption> Options { get; }
}