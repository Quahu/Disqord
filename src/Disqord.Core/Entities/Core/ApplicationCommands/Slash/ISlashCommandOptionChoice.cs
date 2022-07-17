using System.Collections.Generic;
using System.Globalization;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a preset choice for a slash command option.
/// </summary>
public interface ISlashCommandOptionChoice : INamableEntity, IJsonUpdatable<ApplicationCommandOptionChoiceJsonModel>
{
    /// <summary>
    ///     Gets the name localizations of this choice.
    /// </summary>
    /// <remarks>
    ///     Might be empty if retrieved using bulk application command fetch endpoints.
    /// </remarks>
    IReadOnlyDictionary<CultureInfo, string> NameLocalizations { get; }

    /// <summary>
    ///     Gets the value of this choice.
    /// </summary>
    object Value { get; }
}