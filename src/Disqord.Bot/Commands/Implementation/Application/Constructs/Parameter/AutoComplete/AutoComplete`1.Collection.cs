using System.Collections.Generic;
using Qommon.Collections.Proxied;

namespace Disqord.Bot.Commands.Application;

public abstract partial class AutoComplete<T>
{
    /// <summary>
    ///     Represents the collection of choices of <see cref="AutoComplete{T}"/> parameters.
    /// </summary>
    public class ChoiceCollection : ProxiedDictionary<string, T>
    {
        /// <inheritdoc/>
        public ChoiceCollection(int capacity = 0, IEqualityComparer<string>? comparer = null)
            : base(capacity, comparer)
        { }

        /// <inheritdoc/>
        public ChoiceCollection(IDictionary<string, T> dictionary)
            : base(dictionary)
        { }

        /// <summary>
        ///     Adds the specified value as a choice.
        /// </summary>
        /// <remarks>
        ///     The name of the choice is set to the string representation of the value.
        /// </remarks>
        /// <param name="value"> The choice value. </param>
        public virtual void Add(T value)
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
        public virtual void AddRange(params T[] values)
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
        public virtual void AddRange(IEnumerable<T> values)
        {
            foreach (var value in values)
                Add(value.ToString()!, value);
        }

        /// <summary>
        ///     Adds the specified names and values as choices.
        /// </summary>
        /// <param name="values"> The choice name and value pairs. </param>
        public virtual void AddRange(params KeyValuePair<string, T>[] values)
        {
            foreach (var (name, value) in values)
                Add(name, value);
        }

        /// <summary>
        ///     Adds the specified names and values as choices.
        /// </summary>
        /// <param name="values"> The choice name and value pairs. </param>
        public virtual void AddRange(IEnumerable<KeyValuePair<string, T>> values)
        {
            foreach (var (name, value) in values)
                Add(name, value);
        }
    }
}
