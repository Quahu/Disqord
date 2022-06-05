using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Qommon;
using Qommon.Collections.Proxied;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Represents an auto-complete wrapper for a slash command parameter.
/// </summary>
/// <typeparam name="T"> The type of the parameter. </typeparam>
public class AutoComplete<T> : IAutoComplete
    where T : notnull
{
    /// <summary>
    ///     Gets the user's current input for the parameter.
    /// </summary>
    /// <remarks>
    ///     Has no value if the user has not provided any values for the parameter yet.
    /// </remarks>
    public Optional<T> Current { get; }

    /// <inheritdoc/>
    [MemberNotNullWhen(true, nameof(Choices))]
    public bool IsFocused { get; }

    /// <summary>
    ///     Gets the choice collection.
    /// </summary>
    /// <remarks>
    ///     Only valid when <see cref="IsFocused"/> is <see langword="true"/>.
    /// </remarks>
    public ChoiceCollection? Choices { get; }

    Type IAutoComplete.AutoCompletedType => typeof(T);

    /// <summary>
    ///     Instantiates a new <see cref="AutoComplete{T}"/> with the user's current input
    ///     and whether it is currently focused on.
    /// </summary>
    /// <param name="current"> The user's current input. </param>
    /// <param name="isFocused"> Whether this is currently focused on by the user. </param>
    public AutoComplete(Optional<T> current, bool isFocused)
    {
        Current = current;
        IsFocused = isFocused;
        Choices = isFocused ? new ChoiceCollection() : null;
    }

    /// <summary>
    ///     Gets whether this auto-complete parameter is currently focused on by the user.
    ///     If <see langword="true"/>, the <paramref name="currentValue"/> is set to the user's
    ///     current input.
    /// </summary>
    /// <param name="currentValue"> The user's current input for the parameter. </param>
    /// <returns>
    ///     <see langword="true"/> if this parameter is currently focused on by the user.
    /// </returns>
    [MemberNotNullWhen(true, nameof(Choices))]
    public bool IsCurrentlyFocused([MaybeNullWhen(false)] out T currentValue)
    {
        if (IsFocused)
        {
            currentValue = Current.Value;
            return true;
        }

        currentValue = default;
        return false;
    }

    /// <summary>
    ///     Represents the collection of choices of <see cref="AutoComplete{T}"/> parameters.
    /// </summary>
    public class ChoiceCollection : ProxiedDictionary<string, T>
    {
        /// <summary>
        ///     Adds the specified value as a choice.
        /// </summary>
        /// <remarks>
        ///     The name of the choice is set to the string representation of the value.
        /// </remarks>
        /// <param name="value"> The choice value. </param>
        public void Add(T value)
        {
            Add(value.ToString()!, value);
        }

        /// <summary>
        ///     Adds the specified name and value as a choice.
        /// </summary>
        /// <param name="name"> The choice name. </param>
        /// <param name="value"> The choice value. </param>
        public override void Add(string name, T value)
        {
            base.Add(name, value);
        }

        /// <summary>
        ///     Adds the specified values as choices.
        /// </summary>
        /// <remarks>
        ///     The names of the choices are set to the string representations of the values.
        /// </remarks>
        /// <param name="values"> The choice values. </param>
        public void AddRange(params T[] values)
        {
            foreach (var value in values)
                Add(value.ToString()!, value);
        }

        /// <summary>
        ///     Adds the specified names and values as choices.
        /// </summary>
        /// <param name="values"> The choice name and value pairs. </param>
        public void AddRange(params KeyValuePair<string, T>[] values)
        {
            foreach (var (name, value) in values)
                Add(name, value);
        }
    }

    IDictionaryEnumerator IAutoComplete.GetChoiceEnumerator()
    {
        Guard.IsTrue(IsFocused);

        return (Choices as IDictionary).GetEnumerator();
    }
}
