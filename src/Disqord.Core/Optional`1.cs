using System;
using Disqord.Serialization;

namespace Disqord
{
    /// <summary>
    ///     Represents an optional value.
    /// </summary>
    /// <typeparam name="T"> The type of the optional value. </typeparam>
    public readonly struct Optional<T> : IOptional, IEquatable<Optional<T>>
    {
        /// <summary>
        ///     An empty <see cref="Optional{T}"/> instance.
        /// </summary>
        public static readonly Optional<T> Empty = default;

        /// <summary>
        ///     Gets whether this <see cref="Optional{T}"/> has a value.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        ///     Gets the value of this <see cref="Optional{T}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if this optional does not have a value.
        /// </exception>
        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException($"This optional of {GetType().GetGenericArguments()[0]} does not have a value.");

                return _value;
            }
        }

        object IOptional.Value => Value;

        private readonly T _value;

        /// <summary>
        ///     Instantiates a new <see cref="Optional{T}"/> with the specified value.
        /// </summary>
        /// <param name="value"> The specified value. </param>
        public Optional(T value)
        {
            HasValue = true;
            _value = value;
        }

        /// <summary>
        ///     Checks whether this optional is equal to another.
        /// </summary>
        /// <param name="other"> The <see cref="Optional{T}"/> to compare against. </param>
        /// <returns>
        ///     <see langword="true"/> if the optionals are equal.
        /// </returns>
        public bool Equals(Optional<T> other)
        {
            if (!HasValue && !other.HasValue)
                return true;

            if (HasValue != other.HasValue)
                return false;

            return _value.Equals(other._value);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Optional<T> optional)
                return Equals(optional);

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
            => HasValue ? _value.GetHashCode() : -1;

        /// <inheritdoc/>
        public override string ToString()
            => HasValue ? _value?.ToString() ?? "<null>" : "<no value>";

        /// <summary>
        ///     Implicitly converts the <paramref name="value"/> to an <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="value"> The value to convert. </param>
        public static implicit operator Optional<T>(T value)
            => new(value);

        /// <summary>
        ///     Checks whether this optional is equal to another.
        /// </summary>
        /// <param name="left"> The left-hand side <see cref="Optional{T}"/>. </param>
        /// <param name="right"> The right-hand side <see cref="Optional{T}"/> to compare against. </param>
        /// <returns>
        ///     <see langword="true"/> if the optionals are equal.
        /// </returns>
        public static bool operator ==(Optional<T> left, Optional<T> right)
            => left.Equals(right);

        /// <summary>
        ///     Checks whether this optional is equal to another.
        /// </summary>
        /// <param name="left"> The left-hand side <see cref="Optional{T}"/>. </param>
        /// <param name="right"> The right-hand side <see cref="Optional{T}"/> to compare against. </param>
        /// <returns>
        ///     <see langword="true"/> if the optionals are not equal.
        /// </returns>
        public static bool operator !=(Optional<T> left, Optional<T> right)
            => !left.Equals(right);
    }
}
