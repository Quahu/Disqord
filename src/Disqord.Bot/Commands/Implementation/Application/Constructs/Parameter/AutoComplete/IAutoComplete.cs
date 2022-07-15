using System;
using System.Collections;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Represents auto-completion capabilities for a parameter.
/// </summary>
public interface IAutoComplete
{
    /// <summary>
    ///     Gets the auto-completed parameter type.
    /// </summary>
    Type AutoCompletedType { get; }

    /// <summary>
    ///     Gets the user's raw input argument for the parameter,
    ///     i.e. the value the user is currently typing out.
    /// </summary>
    /// <remarks>
    ///     Is <see langword="null"/> if <see cref="IsFocused"/> is <see langword="false"/>.
    ///     <para/>
    ///     Is <see cref="string.Empty"/> if the user has not typed anything yet.
    /// </remarks>
    string? RawArgument { get; }

    /// <summary>
    ///     Gets whether this parameter is currently focused on by the user.
    /// </summary>
    bool IsFocused { get; }

    /// <summary>
    ///     Gets the enumerator yielding the specified choices.
    /// </summary>
    /// <returns>
    ///     An <see cref="IDictionaryEnumerator"/>.
    /// </returns>
    IDictionaryEnumerator GetChoiceEnumerator();
}
