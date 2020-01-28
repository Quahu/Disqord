using System;

namespace Disqord
{
    /// <summary>
    ///     Represents an always present <see cref="Snowflake"/> and an optional <see cref="ISnowflakeEntity"/>.
    /// </summary>
    /// <typeparam name="T"> The type of the entity. </typeparam>
    public readonly struct SnowflakeOptional<T> : IEquatable<T>
        where T : class, ISnowflakeEntity
    {
        /// <inheritdoc cref="ISnowflakeEntity.Id"/>
        public Snowflake Id { get; }

        /// <summary>
        ///     Gets whether this <see cref="SnowflakeOptional{T}"/> has a value.
        /// </summary>
        public bool HasValue => _value != null;

        /// <summary>
        ///     Gets the value of this <see cref="SnowflakeOptional{T}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     This optional snowflake entity does not have a value.
        /// </exception>
        public T Value => _value ?? throw new InvalidOperationException("This optional snowflake entity does not have a value.");
        private readonly T _value;

        internal SnowflakeOptional(Snowflake id)
            : this(null, id)
        { }

        internal SnowflakeOptional(T value)
            : this(value ?? throw new ArgumentNullException(nameof(value)), value.Id)
        { }

        internal SnowflakeOptional(T value, Snowflake id)
        {
            _value = value;
            Id = id;
        }

        /// <summary>
        ///     Returns a hash code for this <see cref="SnowflakeOptional{T}"/>.
        /// </summary>
        /// <returns>
        ///     The hash code.
        /// </returns>
        public override int GetHashCode()
            => _value?.GetHashCode() ?? -1;

        /// <summary>
        ///     Returns the string representation of this <see cref="SnowflakeOptional{T}"/>.
        /// </summary>
        /// <returns>
        ///     The string representation of this <see cref="SnowflakeOptional{T}"/>.
        /// </returns>
        public override string ToString()
            => _value?.ToString() ?? "<no value>";

        /// <summary>
        ///     Checks whether this <see cref="SnowflakeOptional{T}"/> or this <see cref="Value"/> are equal to the specified <see cref="object"/>.
        /// </summary>
        /// <param name="obj"> The <see cref="object"/> to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public override bool Equals(object obj)
            => obj is T value && Equals(value);

        /// <summary>
        ///     Checks whether this <see cref="Value"/> is equal to the specified <typeparamref name="T"/> value.
        /// </summary>
        /// <param name="other"> The <typeparamref name="T"/> value to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public bool Equals(T other)
            => !HasValue && other == null || HasValue && ReferenceEquals(_value, other);

        public static bool operator ==(SnowflakeOptional<T> left, T right)
            => left.Equals(right);

        public static bool operator !=(SnowflakeOptional<T> left, T right)
            => !left.Equals(right);
    }
}
