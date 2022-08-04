using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Qommon;

namespace Disqord.Bot.Commands.Application;

/// <inheritdoc cref="IAutoComplete{T}"/>
/// <typeparam name="T"> The type of the parameter. </typeparam>
public abstract partial class AutoComplete<T> : IAutoComplete<T>
    where T : notnull
{
    /// <inheritdoc/>
    [MemberNotNullWhen(true, nameof(RawArgument), nameof(Choices))]
    public abstract bool IsFocused { get; }

    /// <inheritdoc/>
    public abstract string? RawArgument { get; }

    /// <inheritdoc/>
    public abstract Optional<T> Argument { get; }

    /// <summary>
    ///     Gets the choice collection of this parameter.
    /// </summary>
    /// <remarks>
    ///     Is <see langword="null"/> if <see cref="IsFocused"/> is <see langword="false"/>
    ///     as choices can only be provided for the currently focused parameter.
    /// </remarks>
    public abstract ChoiceCollection? Choices { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return IsFocused ? $"{nameof(RawArgument)}: \"{RawArgument}\"" : $"{nameof(Argument)}: {Argument}";
    }

    IDictionaryEnumerator IAutoComplete.EnumerateChoices()
    {
        Guard.IsTrue(IsFocused);

        return (Choices as IDictionary).GetEnumerator();
    }
}
