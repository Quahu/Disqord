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
    ///     Gets the user's input argument for the parameter.
    /// </summary>
    /// <remarks>
    ///     Has no value if the user has not provided any values for the parameter yet.
    ///     <para/>
    ///     Has no value if <see cref="IsFocused"/> is <see langword="true"/>.
    /// </remarks>
    public Optional<T> Argument { get; }

    /// <inheritdoc/>
    public string? RawArgument { get; }

    /// <inheritdoc/>
    [MemberNotNullWhen(true, nameof(Choices))]
    [MemberNotNullWhen(true, nameof(RawArgument))]
    public bool IsFocused { get; }

    /// <summary>
    ///     Gets the choice collection.
    /// </summary>
    /// <remarks>
    ///     Is <see langword="null"/> when <see cref="IsFocused"/> is <see langword="false"/>.
    /// </remarks>
    public ChoiceCollection? Choices { get; }

    Type IAutoComplete.AutoCompletedType => typeof(T);

    /// <summary>
    ///     Instantiates a new <see cref="AutoComplete{T}"/> with the user's input argument.
    /// </summary>
    /// <param name="argument"> The user's input argument. </param>
    public AutoComplete(Optional<T> argument)
    {
        Argument = argument;
    }

    /// <summary>
    ///     Instantiates a new <see cref="AutoComplete{T}"/> with the user's raw input argument.
    /// </summary>
    /// <param name="rawArgument"> The user's raw input argument. </param>
    public AutoComplete(string rawArgument)
    {
        RawArgument = rawArgument;
        IsFocused = true;
        Choices = new ChoiceCollection();
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
        ///     Adds the specified values as choices.
        /// </summary>
        /// <remarks>
        ///     The names of the choices are set to the string representations of the values.
        /// </remarks>
        /// <param name="values"> The choice values. </param>
        public void AddRange(IEnumerable<T> values)
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

        /// <summary>
        ///     Adds the specified names and values as choices.
        /// </summary>
        /// <param name="values"> The choice name and value pairs. </param>
        public void AddRange(IEnumerable<KeyValuePair<string, T>> values)
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
