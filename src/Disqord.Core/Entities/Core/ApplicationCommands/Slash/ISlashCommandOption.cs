using System.Collections.Generic;
using System.Globalization;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a slash command option.
/// </summary>
public interface ISlashCommandOption : INamableEntity, IJsonUpdatable<ApplicationCommandOptionJsonModel>
{
    /// <summary>
    ///     Gets the type of this option.
    /// </summary>
    SlashCommandOptionType Type { get; }

    /// <summary>
    ///     Gets the name localizations of this option.
    /// </summary>
    /// <remarks>
    ///     Might be empty if retrieved using bulk application command fetch endpoints.
    /// </remarks>
    IReadOnlyDictionary<CultureInfo, string> NameLocalizations { get; }

    /// <summary>
    ///     Gets the description of this option.
    /// </summary>
    string Description { get; }

    /// <summary>
    ///     Gets the description localizations of this option.
    /// </summary>
    /// <remarks>
    ///     Might be empty if retrieved using bulk application command fetch endpoints.
    /// </remarks>
    IReadOnlyDictionary<CultureInfo, string> DescriptionLocalizations { get; }

    /// <summary>
    ///     Gets whether this option is required.
    /// </summary>
    bool IsRequired { get; }

    /// <summary>
    ///     Gets the choices of this option.
    /// </summary>
    IReadOnlyList<ISlashCommandOptionChoice> Choices { get; }

    /// <summary>
    ///     Gets the nested options of this option.
    /// </summary>
    IReadOnlyList<ISlashCommandOption> Options { get; }

    /// <summary>
    ///     Gets the channel types this option is restricted to.
    /// </summary>
    IReadOnlyList<ChannelType> ChannelTypes { get; }

    /// <summary>
    ///     Gets the minimum integer/number value this option allows.
    /// </summary>
    double? MinimumValue { get; }

    /// <summary>
    ///     Gets the maximum integer/number value this option allows.
    /// </summary>
    double? MaximumValue { get; }

    /// <summary>
    ///     Gets the minimum length of the value this option allows.
    /// </summary>
    int? MinimumLength { get; }

    /// <summary>
    ///     Gets the maximum length of the value this option allows.
    /// </summary>
    int? MaximumLength { get; }

    /// <summary>
    ///     Gets whether this option supports auto-complete.
    /// </summary>
    bool HasAutoComplete { get; }
}
