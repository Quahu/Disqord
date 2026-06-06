using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord;

/// <summary>
///     Represents a provided value for a slash command option.
/// </summary>
public interface ISlashCommandInteractionOption
{
    /// <summary>
    ///     Gets the name of this option.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Gets the type of this option.
    /// </summary>
    SlashCommandOptionType Type { get; }

    /// <summary>
    ///     Gets the value of this option.
    /// </summary>
    /// <remarks>
    ///     <see cref="Value"/> and <see cref="Options"/> are mutually exclusive.
    /// </remarks>
    IJsonValue? Value { get; }

    /// <summary>
    ///     Gets the nested options of this option.
    /// </summary>
    /// <remarks>
    ///     <see cref="Value"/> and <see cref="Options"/> are mutually exclusive.
    /// </remarks>
    IReadOnlyDictionary<string, ISlashCommandInteractionOption> Options { get; }
}
