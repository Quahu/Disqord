using System;
using Disqord.Serialization;

namespace Disqord
{
    /// <summary>
    ///     Represents an optional value.
    /// </summary>
    /// <typeparam name="T"> The type of the optional value. </typeparam>
    public readonly struct Optional<T> : IOptional, IEquatable<T>, IEquatable<Optional<T>>
    {
        /// <summary>
        ///     An empty <see cref="Optional{T}"/> instance.
        /// </summary>
        public static readonly Optional<T> Empty = new Optional<T>();

        /// <summary>
        ///     Gets whether this <see cref="Optional{T}"/> has a value.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        ///     Gets the value of this <see cref="Optional{T}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     This optional doesn't have a value.
        /// </exception>
        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException("This optional does not have a value.");

                return _value;
            }
        }

        object IOptional.Value => Value;

        private readonly T _value;

        /// <summary>
        ///     Initialises a new <see cref="Optional{T}"/> with the specified value.
        /// </summary>
        /// <param name="value"> The specified value. </param>
        public Optional(T value)
        {
            HasValue = true;
            _value = value;
        }

        /// <summary>
        ///     Retrieves the value of this <see cref="Optional{T}"/>, or the default value of <typeparamref name="T"/>.
        /// </summary>
        /// <returns>
        ///     The value of this <see cref="Optional{T}"/> or the default value of <typeparamref name="T"/>.
        /// </returns>
        public T GetValueOrDefault()
            => HasValue ? _value : default;

        /// <summary>
        ///     Retrieves the value of this <see cref="Optional{T}"/>, or the provided default value.
        /// </summary>
        /// <returns>
        ///     The value of this <see cref="Optional{T}"/> or the provided default value.
        /// </returns>
        public T GetValueOrDefault(T value)
            => HasValue ? _value : value;

        /// <summary>
        ///     Retrieves the value of this <see cref="Optional{T}"/>, or the default value from the provided factory.
        /// </summary>
        /// <returns>
        ///     The value of this <see cref="Optional{T}"/> or the default value from the provided factory.
        /// </returns>
        public T GetValueOrDefault(Func<T> value)
            => HasValue ? _value : (value ?? throw new ArgumentNullException(nameof(value)))();

        /// <summary>
        ///     Retrieves the value of this <see cref="Optional{T}"/>, or the default value from the provided factory.
        /// </summary>
        /// <returns>
        ///     The value of this <see cref="Optional{T}"/> or the default value from the provided factory.
        /// </returns>
        public T GetValueOrDefault<TState>(Func<TState, T> value, TState state)
            => HasValue ? _value : (value ?? throw new ArgumentNullException(nameof(value)))(state);

        /// <summary>
        ///     Returns a hash code for this <see cref="Optional{T}"/>.
        /// </summary>
        /// <returns>
        ///     The hash code.
        /// </returns>
        public override int GetHashCode()
            => HasValue ? _value.GetHashCode() : -1;

        /// <summary>
        ///     Returns the string representation of this <see cref="Optional{T}"/>.
        /// </summary>
        /// <returns>
        ///     The string representation of this <see cref="Optional{T}"/>.
        /// </returns>
        public override string ToString()
            => HasValue ? _value?.ToString() ?? "<null>" : "<no value>";

        /// <summary>
        ///     Checks whether this <see cref="Optional{T}"/> or this <see cref="Value"/> are equal to the specified <see cref="object"/>.
        /// </summary>
        /// <param name="obj"> The <see cref="object"/> to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is T value)
                return Equals(value);

            if (obj is Optional<T> optional)
                return Equals(optional);

            return false;
        }

        /// <summary>
        ///     Checks whether this <see cref="Value"/> is equal to the specified <typeparamref name="T"/> value.
        /// </summary>
        /// <param name="other"> The <typeparamref name="T"/> value to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public bool Equals(T other)
        {
            if (!HasValue)
                return false;

            return ReferenceEquals(_value, other);
        }

        /// <summary>
        ///     Checks whether this <see cref="Value"/> is equal to the specified <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="other"> The <see cref="Optional{T}"/> to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public bool Equals(Optional<T> other)
        {
            if (!HasValue && !other.HasValue)
                return true;

            if (HasValue != other.HasValue)
                return false;

            return _value.Equals(other._value);
        }

        /// <summary>
        ///     Implicitly converts the <paramref name="value"/> to an <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="value"> The value to convert. </param>
        public static implicit operator Optional<T>(T value)
            => new Optional<T>(value);

        /// <summary>
        ///     Implicitly gets the <see cref="Optional{T}.Value"/>.
        /// </summary>
        /// <param name="value"> The optional to get the value from. </param>
        /// <exception cref="InvalidOperationException">
        ///     This optional doesn't have a value.
        /// </exception>
        public static explicit operator T(Optional<T> value)
            => value._value;

        /// <summary>
        ///     Checks if this <see cref="Optional{T}"/> is equal to another <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="left"> The left-hand side <see cref="Optional{T}"/>. </param>
        /// <param name="right"> The right-hand side <see cref="Optional{T}"/> to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public static bool operator ==(Optional<T> left, Optional<T> right)
            => left.Equals(right);

        /// <summary>
        ///     Checks if this <see cref="Optional{T}"/> isn't equal to another <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="left"> The left-hand side <see cref="Optional{T}"/>. </param>
        /// <param name="right"> The right-hand side <see cref="Optional{T}"/> to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public static bool operator !=(Optional<T> left, Optional<T> right)
            => !left.Equals(right);

        public static bool operator ==(Optional<T> left, T right)
            => left.Equals(right);

        public static bool operator !=(Optional<T> left, T right)
            => !left.Equals(right);

        public static bool operator ==(T left, Optional<T> right)
            => right.Equals(left);

        public static bool operator !=(T left, Optional<T> right)
            => !right.Equals(left);
    }
}
