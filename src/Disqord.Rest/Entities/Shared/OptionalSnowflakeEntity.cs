using System;

namespace Disqord
{
    /// <summary>
    ///     Represents an always present <see cref="Snowflake"/> and an optional <see cref="ISnowflakeEntity"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OptionalSnowflakeEntity<T> : IEquatable<OptionalSnowflakeEntity<T>>, IEquatable<T>
        where T : class, ISnowflakeEntity
    {
        /// <summary>
        ///     Gets the <see cref="Snowflake"/> of the entity.
        /// </summary>
        public Snowflake Id { get; }

        /// <summary>
        ///     Gets whether this <see cref="OptionalSnowflakeEntity{T}"/> has a value.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        ///     Gets the value of this <see cref="OptionalSnowflakeEntity{T}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     This optional snowflake entity does not have a value.
        /// </exception>
        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException("This optional snowflake entity does not have a value.");

                return _value;
            }
        }
        private readonly T _value;

        internal OptionalSnowflakeEntity(Snowflake id)
        {
            Id = id;
            _value = default;
            HasValue = false;
        }

        internal OptionalSnowflakeEntity(T value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            Id = value.Id;
            HasValue = true;
        }

        internal OptionalSnowflakeEntity(T value, Snowflake id)
        {
            _value = value;
            Id = id;
            HasValue = value != null;
        }

        /// <summary>
        ///     Returns a hash code for this <see cref="OptionalSnowflakeEntity{T}"/>.
        /// </summary>
        /// <returns>
        ///     The hash code.
        /// </returns>
        public override int GetHashCode()
            => HasValue ? _value.GetHashCode() : -1;

        /// <summary>
        ///     Returns the string representation of this <see cref="OptionalSnowflakeEntity{T}"/>.
        /// </summary>
        /// <returns>
        ///     The string representation of this <see cref="OptionalSnowflakeEntity{T}"/>.
        /// </returns>
        public override string ToString()
            => HasValue ? _value.ToString() : "<no value>";

        /// <summary>
        ///     Checks whether this <see cref="OptionalSnowflakeEntity{T}"/> or this <see cref="Value"/> are equal to the specified <see cref="object"/>.
        /// </summary>
        /// <param name="obj"> The <see cref="object"/> to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is T value)
                return Equals(value);

            if (obj is OptionalSnowflakeEntity<T> Cachable)
                return Equals(Cachable);

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
            if (!HasValue && other == null)
                return true;

            if (!HasValue)
                return false;

            return ReferenceEquals(_value, other);
        }

        /// <summary>
        ///     Checks whether this <see cref="Value"/> is equal to the specified <see cref="OptionalSnowflakeEntity{T}"/>.
        /// </summary>
        /// <param name="other"> The <see cref="OptionalSnowflakeEntity{T}"/> to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public bool Equals(OptionalSnowflakeEntity<T> other)
        {
            if (!HasValue && !other.HasValue)
                return true;

            if (HasValue != other.HasValue)
                return false;

            return _value.Equals(other._value);
        }

        /// <summary>
        ///     Checks if this <see cref="OptionalSnowflakeEntity{T}"/> is equal to another <see cref="OptionalSnowflakeEntity{T}"/>.
        /// </summary>
        /// <param name="left"> The left-hand side <see cref="OptionalSnowflakeEntity{T}"/>. </param>
        /// <param name="right"> The right-hand side <see cref="OptionalSnowflakeEntity{T}"/> to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public static bool operator ==(OptionalSnowflakeEntity<T> left, OptionalSnowflakeEntity<T> right)
            => left.Equals(right);

        /// <summary>
        ///     Checks if this <see cref="OptionalSnowflakeEntity{T}"/> isn't equal to another <see cref="OptionalSnowflakeEntity{T}"/>.
        /// </summary>
        /// <param name="left"> The left-hand side <see cref="OptionalSnowflakeEntity{T}"/>. </param>
        /// <param name="right"> The right-hand side <see cref="OptionalSnowflakeEntity{T}"/> to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public static bool operator !=(OptionalSnowflakeEntity<T> left, OptionalSnowflakeEntity<T> right)
            => !left.Equals(right);

        public static bool operator ==(OptionalSnowflakeEntity<T> left, T right)
            => left.Equals(right);

        public static bool operator !=(OptionalSnowflakeEntity<T> left, T right)
            => !left.Equals(right);
    }
}
