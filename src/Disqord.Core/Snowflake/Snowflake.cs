using System;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a Discord snowflake, i.e. a <see cref="ulong"/> offset by the constant <see cref="Epoch"/>.
/// </summary>
/// <remarks>
///     Implicitly casts to and from <see cref="ulong"/>.
/// </remarks>
public readonly partial struct Snowflake : IConvertible, ISpanFormattable, IComparable,
    IEquatable<Snowflake>, IComparable<Snowflake>,
    IEquatable<ulong>, IComparable<ulong>
{
    /// <summary>
    ///     Represents the constant epoch.
    /// </summary>
    public const ulong Epoch = 1420070400000;

    /// <summary>
    ///     Gets the wrapped <see cref="ulong"/> value.
    /// </summary>
    public ulong RawValue { get; }

    /// <summary>
    ///     Gets when this snowflake was created at.
    /// </summary>
    public DateTimeOffset CreatedAt => DateTimeOffset.FromUnixTimeMilliseconds((long) ((RawValue >> 22) + Epoch));

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

    /// <summary>
    ///     Instantiates a new <see cref="Snowflake"/> from the specified <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="createdAt"> The creation date to create the snowflake from. </param>
    public Snowflake(DateTimeOffset createdAt)
    {
        RawValue = ((ulong) createdAt.ToUnixTimeMilliseconds() - Epoch) << 22;
    }

    /// <inheritdoc/>
    public bool Equals(Snowflake other)
    {
        return RawValue == other.RawValue;
    }

    /// <inheritdoc/>
    public int CompareTo(Snowflake other)
    {
        return RawValue.CompareTo(other.RawValue);
    }

    /// <inheritdoc/>
    public bool Equals(ulong other)
    {
        return RawValue == other;
    }

    /// <inheritdoc/>
    public int CompareTo(ulong other)
    {
        return RawValue.CompareTo(other);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is Snowflake otherSnowflake)
            return RawValue == otherSnowflake.RawValue;

        if (obj is ulong otherRawValue)
            return RawValue == otherRawValue;

        return false;
    }

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        if (obj == null)
            return 1;

        if (obj is Snowflake otherSnowflake)
            return RawValue.CompareTo(otherSnowflake.RawValue);

        if (obj is ulong otherRawValue)
            return RawValue.CompareTo(otherRawValue);

        return Throw.ArgumentException<int>("Argument must be a Snowflake or ulong.", nameof(obj));
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return RawValue.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return RawValue.ToString();
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        return RawValue.ToString(format, formatProvider);
    }

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        return RawValue.TryFormat(destination, out charsWritten, format, provider);
    }

    /// <inheritdoc cref="TryParse(ReadOnlySpan{char},out Snowflake)"/>
    public static bool TryParse(string? value, out Snowflake result)
    {
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
        Guard.IsNotNull(value);

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

        if (value.Length < 15 || value.Length >= 21)
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
    [Obsolete("Use new Snowflake(DateTimeOffset) instead.")]
    public static Snowflake FromDateTimeOffset(DateTimeOffset dateTimeOffset)
    {
        return new Snowflake(dateTimeOffset);
    }

    /// <summary>
    ///     Converts a <see cref="ulong"/> to a <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="id"> The value to convert. </param>
    /// <returns>
    ///     The converted <see cref="DateTimeOffset"/>.
    /// </returns>
    [Obsolete("Use new Snowflake(UInt64).CreatedAt instead.")]
    public static DateTimeOffset ToDateTimeOffset(ulong id)
    {
        return new Snowflake(id).CreatedAt;
    }
}
