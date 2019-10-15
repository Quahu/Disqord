using System;
using Disqord.Rest;

namespace Disqord
{
    public sealed class DownloadableOptionalSnowflakeEntity<T, TDownloadable> : OptionalSnowflakeEntity<T>, IEquatable<DownloadableOptionalSnowflakeEntity<T, TDownloadable>>
        where T : CachedSnowflakeEntity
        where TDownloadable : RestSnowflakeEntity
    {
        public RestDownloadable<TDownloadable> Downloadable { get; }

        internal DownloadableOptionalSnowflakeEntity(Snowflake id, RestDownloadableDelegate<TDownloadable> func)
            : this(null, id, func)
        { }

        internal DownloadableOptionalSnowflakeEntity(T value, RestDownloadableDelegate<TDownloadable> func)
            : this(value, value.Id, func)
        { }

        internal DownloadableOptionalSnowflakeEntity(T value, Snowflake id, RestDownloadableDelegate<TDownloadable> func)
            : base(value, id)
        {
            Downloadable = new RestDownloadable<TDownloadable>(func);
        }

        /// <summary>
        ///     Returns a hash code for this <see cref="DownloadableOptionalSnowflakeEntity{T, TDownloadable}"/>.
        /// </summary>
        /// <returns>
        ///     The hash code.
        /// </returns>
        public override int GetHashCode()
            => HasValue ? Value.GetHashCode() : -1;

        /// <summary>
        ///     Returns the string representation of this <see cref="DownloadableOptionalSnowflakeEntity{T, TDownloadable}"/>.
        /// </summary>
        /// <returns>
        ///     The string representation of this <see cref="DownloadableOptionalSnowflakeEntity{T, TDownloadable}"/>.
        /// </returns>
        public override string ToString()
            => HasValue ? Value.ToString() : "<no value>";

        /// <summary>
        ///     Checks whether this <see cref="DownloadableOptionalSnowflakeEntity{T, TDownloadable}"/> or this <see cref="Value"/> are equal to the specified <see cref="object"/>.
        /// </summary>
        /// <param name="obj"> The <see cref="object"/> to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is T value)
                return Equals(value);

            if (obj is DownloadableOptionalSnowflakeEntity<T, TDownloadable> Cachable)
                return Equals(Cachable);

            return false;
        }

        /// <summary>
        ///     Checks whether this <see cref="Value"/> is equal to the specified <see cref="DownloadableOptionalSnowflakeEntity{T, TDownloadable}"/>.
        /// </summary>
        /// <param name="other"> The <see cref="DownloadableOptionalSnowflakeEntity{T, TDownloadable}"/> to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public bool Equals(DownloadableOptionalSnowflakeEntity<T, TDownloadable> other)
        {
            if (!HasValue && !other.HasValue)
                return true;

            if (HasValue != other.HasValue)
                return false;

            return Value.Equals(other.Value);
        }

        /// <summary>
        ///     Checks if this <see cref="DownloadableOptionalSnowflakeEntity{T, TDownloadable}"/> is equal to another <see cref="DownloadableOptionalSnowflakeEntity{T, TDownloadable}"/>.
        /// </summary>
        /// <param name="left"> The left-hand side <see cref="DownloadableOptionalSnowflakeEntity{T, TDownloadable}"/>. </param>
        /// <param name="right"> The right-hand side <see cref="DownloadableOptionalSnowflakeEntity{T, TDownloadable}"/> to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public static bool operator ==(DownloadableOptionalSnowflakeEntity<T, TDownloadable> left, DownloadableOptionalSnowflakeEntity<T, TDownloadable> right)
            => left.Equals(right);

        /// <summary>
        ///     Checks if this <see cref="DownloadableOptionalSnowflakeEntity{T, TDownloadable}"/> isn't equal to another <see cref="DownloadableOptionalSnowflakeEntity{T, TDownloadable}"/>.
        /// </summary>
        /// <param name="left"> The left-hand side <see cref="DownloadableOptionalSnowflakeEntity{T, TDownloadable}"/>. </param>
        /// <param name="right"> The right-hand side <see cref="DownloadableOptionalSnowflakeEntity{T, TDownloadable}"/> to compare against. </param>
        /// <returns>
        ///     The <see cref="bool"/> value reresenting whether the comparison succeeded.
        /// </returns>
        public static bool operator !=(DownloadableOptionalSnowflakeEntity<T, TDownloadable> left, DownloadableOptionalSnowflakeEntity<T, TDownloadable> right)
            => !left.Equals(right);

        public static bool operator ==(DownloadableOptionalSnowflakeEntity<T, TDownloadable> left, T right)
            => left.Equals(right);

        public static bool operator !=(DownloadableOptionalSnowflakeEntity<T, TDownloadable> left, T right)
            => !left.Equals(right);
    }
}
