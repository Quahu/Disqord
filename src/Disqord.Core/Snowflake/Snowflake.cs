using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a Discord snowflake, i.e. a <see cref="ulong"/> offset by the constant <see cref="Epoch"/>.
    ///     <see cref="Snowflake"/> can be implicitly casted to and from <see cref="ulong"/>.
    /// </summary>
    public readonly partial struct Snowflake : IConvertible, IEquatable<ulong>, IEquatable<Snowflake>, IComparable<ulong>, IComparable<Snowflake>
    {
        /// <summary>
        ///     Gets the constant epoch.
        /// </summary>
        public const ulong Epoch = 1420070400000;

        /// <summary>
        ///     Gets the wrapped <see cref="ulong"/> value.
        /// </summary>
        public ulong RawValue { get; }

        /// <summary>
        ///     Gets when this snowflake was created at.
        /// </summary>
        public DateTimeOffset CreatedAt => ToDateTimeOffset(RawValue);

        /// <summary>
        ///     Gets the internal worker ID of this snowflake.
        /// </summary>
        public byte InternalWorkerId => (byte) ((RawValue & 0x3E0000) >> 17);

        /// <summary>
        ///     Gets the internal process ID of this snowflake.
        /// </summary>
        public byte InternalProcessId => (byte) ((RawValue & 0x1F000) >> 12);

        /// <summary>
        ///     Gets the increment of this snowflake.
        /// </summary>
        public ushort Increment => (ushort) (RawValue & 0xFFF);

        /// <summary>
        ///     Instantiates a new <see cref="Snowflake"/> with the specified <see cref="ulong"/> value.
        /// </summary>
        /// <param name="rawValue"> The <see cref="ulong"/> to wrap. </param>
        public Snowflake(ulong rawValue)
        {
            RawValue = rawValue;
        }

        /// <inheritdoc/>
        public bool Equals(ulong other)
            => RawValue == other;

        /// <inheritdoc/>
        public bool Equals(Snowflake other)
            => RawValue == other.RawValue;

        /// <inheritdoc/>
        public int CompareTo(ulong other)
            => RawValue.CompareTo(other);

        /// <inheritdoc/>
        public int CompareTo(Snowflake other)
            => RawValue.CompareTo(other.RawValue);

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Snowflake otherSnowflake)
                return RawValue == otherSnowflake.RawValue;

            if (obj is ulong otherRawValue)
                return RawValue == otherRawValue;

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
            => RawValue.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => RawValue.ToString();

        /// <inheritdoc cref="TryParse(ReadOnlySpan{char},out Snowflake)"/>
        public static bool TryParse(string value, out Snowflake result)
        {
            // TODO: change with NRT
            // Guard.IsNotNull(value);

            return TryParse(value.AsSpan(), out result);
        }

        /// <summary>
        ///     Attempts to convert the input to a <see cref="Snowflake"/>.
        /// </summary>
        /// <param name="value"> The input to convert. </param>
        /// <param name="result"> The <see cref="Snowflake"/> equivalent of the input. </param>
        /// <exception cref="FormatException">
        ///     The input was in an incorrect format.
        /// </exception>
        public static bool TryParse(ReadOnlySpan<char> value, out Snowflake result)
        {
            if (value.Length >= 15 && value.Length < 21 && ulong.TryParse(value, out var ulongResult))
            {
                result = ulongResult;
                return true;
            }

            result = default;
            return false;
        }

        /// <inheritdoc cref="Parse(ReadOnlySpan{char})"/>
        public static Snowflake Parse(string value)
        {
            // TODO: change with NRT
            // Guard.IsNotNull(value);

            return Parse(value.AsSpan());
        }

        /// <summary>
        ///     Converts the input to a <see cref="Snowflake"/>.
        /// </summary>
        /// <param name="value"> The input to convert. </param>
        /// <returns>
        ///     The <see cref="Snowflake"/> equivalent of the input.
        /// </returns>
        /// <exception cref="FormatException">
        ///     The input was in an incorrect format.
        /// </exception>
        public static Snowflake Parse(ReadOnlySpan<char> value)
        {
            const string exceptionMessage = "The input was in an incorrect format.";

            if (value.Length >= 15 && value.Length < 21)
                throw new FormatException(exceptionMessage);

            try
            {
                return ulong.Parse(value);
            }
            catch (Exception ex)
            {
                throw new FormatException(exceptionMessage, ex);
            }
        }

        /// <summary>
        ///     Converts a <see cref="DateTimeOffset"/> to a <see cref="Snowflake"/>.
        /// </summary>
        /// <param name="dateTimeOffset"> The value to convert. </param>
        /// <returns>
        ///     The converted <see cref="Snowflake"/>.
        /// </returns>
        public static Snowflake FromDateTimeOffset(DateTimeOffset dateTimeOffset)
            => ((ulong) dateTimeOffset.ToUniversalTime().ToUnixTimeMilliseconds() - Epoch) << 22;

        /// <summary>
        ///     Converts a <see cref="ulong"/> to a <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="id"> The value to convert. </param>
        /// <returns>
        ///     The converted <see cref="DateTimeOffset"/>.
        /// </returns>
        public static DateTimeOffset ToDateTimeOffset(ulong id)
            => DateTimeOffset.FromUnixTimeMilliseconds((long) ((id >> 22) + Epoch));
    }
}
