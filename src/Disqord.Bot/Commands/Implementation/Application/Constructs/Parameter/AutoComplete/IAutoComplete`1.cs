using System;
using Qommon;

namespace Disqord.Bot.Commands.Application;

/// <inheritdoc/>
public interface IAutoComplete<T> : IAutoComplete
    where T : notnull
{
    Type IAutoComplete.AutoCompletedType => typeof(T);

    /// <summary>
    ///     Gets the user's input argument for this parameter,
    ///     i.e. the value the user had typed out and then unfocused this parameter.
    /// </summary>
    /// <remarks>
    ///     Has no value if <see cref="IAutoComplete.IsFocused"/> is <see langword="true"/>.
    ///     In that case <see cref="IAutoComplete.RawArgument"/> is used instead.
    ///     <para/>
    ///     Has no value if the user has not provided any values for this parameter yet.
    /// </remarks>
    Optional<T> Argument { get; }
}
