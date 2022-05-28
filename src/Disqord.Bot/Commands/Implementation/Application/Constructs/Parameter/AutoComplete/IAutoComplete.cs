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
    ///     Gets whether this parameter is currently focused by the user.
    /// </summary>
    bool IsFocused { get; }

    /// <summary>
    ///     Gets the enumerator yielding the specified choices.
    /// </summary>
    /// <returns></returns>
    IDictionaryEnumerator GetChoiceEnumerator();
}
