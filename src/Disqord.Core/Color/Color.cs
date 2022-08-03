using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a Discord color, i.e. a packed (<c>24</c>-bit) RGB color value.
/// </summary>
/// <remarks>
///     Does not support transparency.
/// </remarks>
public readonly partial struct Color : ISpanFormattable, IComparable,
    IEquatable<Color>, IComparable<Color>,
    IEquatable<int>, IComparable<int>
{
    /// <summary>
    ///     Gets the raw value of this color, i.e. the packed RGB value.
    /// </summary>
    public int RawValue { get; }

    /// <summary>
    ///     Gets the red component of this color.
    /// </summary>
    public byte R
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (byte) (RawValue >> 16);
    }

    /// <summary>
    ///     Gets the green component of this color.
    /// </summary>
    public byte G
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (byte) (RawValue >> 8);
    }

    /// <summary>
    ///     Gets the blue component of this color.
    /// </summary>
    public byte B
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (byte) RawValue;
    }

    /// <summary>
    ///     Instantiates a new <see cref="Color"/> using the given raw value.
    /// </summary>
    /// <param name="rawValue"> The raw value. </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     "Raw value must be a non-negative value less than or equal to <c>16777215</c>."
    /// </exception>
    public Color(int rawValue)
    {
        Guard.IsBetweenOrEqualTo(rawValue, 0, 0xFFFFFF);

        RawValue = rawValue;
    }

    /// <summary>
    ///     Instantiates a new <see cref="Color"/> using the given RGB values.
    /// </summary>
    /// <param name="r"> The red component. </param>
    /// <param name="g"> The green component. </param>
    /// <param name="b"> The blue component. </param>
    public Color(byte r, byte g, byte b)
    {
        RawValue = r << 16 | g << 8 | b;
    }

    /// <summary>
    ///     Instantiates a new <see cref="Color"/> using the given RGB values.
    /// </summary>
    /// <remarks>
    ///     The values must be in the range
    /// </remarks>
    /// <param name="r"> The red component. </param>
    /// <param name="g"> The green component. </param>
    /// <param name="b"> The blue component. </param>
    public Color(float r, float g, float b)
    {
        Guard.IsBetweenOrEqualTo(r, 0, 1);
        Guard.IsBetweenOrEqualTo(g, 0, 1);
        Guard.IsBetweenOrEqualTo(b, 0, 1);

        RawValue = (byte) (r * 255) << 16 | (byte) (g * 255) << 8 | (byte) (b * 255);
    }

    /// <inheritdoc/>
    public bool Equals(Color other)
    {
        return RawValue == other.RawValue;
    }

    /// <inheritdoc/>
    public int CompareTo(Color other)
    {
        return RawValue.CompareTo(other.RawValue);
    }

    /// <inheritdoc/>
    public bool Equals(int other)
    {
        return RawValue == other;
    }

    /// <inheritdoc/>
    public int CompareTo(int other)
    {
        return RawValue.CompareTo(other);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Color other && Equals(other);
    }

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        if (obj == null)
            return 1;

        if (obj is Color other)
            return CompareTo(other);

        return Throw.ArgumentException<int>("Argument must be a Color.", nameof(obj));
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return RawValue;
    }

    /// <summary>
    ///     Returns the hexadecimal representation of this <see cref="Color"/>.
    /// </summary>
    /// <returns>
    ///     The hexadecimal representation of this <see cref="Color"/>.
    /// </returns>
    public override string ToString()
    {
        return $"#{RawValue:X6}";
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        if (format == null)
            return $"#{RawValue.ToString("X6", formatProvider)}";

        return RawValue.ToString(format, formatProvider);
    }

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        if (destination.Length == 0)
        {
            charsWritten = 0;
            return false;
        }

        if (!format.IsEmpty)
            return RawValue.TryFormat(destination, out charsWritten, format, provider);

        var result = RawValue.TryFormat(destination[1..], out charsWritten, "X6", provider);
        if (result)
        {
            destination[0] = '#';
            charsWritten++;
        }

        return result;
    }

    /// <summary>
    ///     Deconstructs this color into RGB.
    /// </summary>
    /// <param name="r"> The red component. </param>
    /// <param name="g"> The green component. </param>
    /// <param name="b"> The blue component. </param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out byte r, out byte g, out byte b)
    {
        r = R;
        g = G;
        b = B;
    }

    public static bool operator <(Color left, Color right)
    {
        return left.RawValue < right.RawValue;
    }

    public static bool operator <=(Color left, Color right)
    {
        return left.RawValue <= right.RawValue;
    }

    public static bool operator >(Color left, Color right)
    {
        return left.RawValue > right.RawValue;
    }

    public static bool operator >=(Color left, Color right)
    {
        return left.RawValue >= right.RawValue;
    }

    public static bool operator ==(Color left, Color right)
    {
        return left.RawValue == right.RawValue;
    }

    public static bool operator !=(Color left, Color right)
    {
        return left.RawValue != right.RawValue;
    }

    /// <summary>
    ///     Implicitly instantiates a new <see cref="Color"/> from this raw value.
    /// </summary>
    /// <param name="value"> The raw value. </param>
    public static implicit operator Color(int value)
    {
        return new(value);
    }

    /// <summary>
    ///     Implicitly gets <see cref="RawValue"/> from the given <see cref="Color"/>.
    /// </summary>
    /// <param name="value"> The color value. </param>
    public static implicit operator int(Color value)
    {
        return value.RawValue;
    }

    /// <summary>
    ///     Implicitly converts the RGB tuple to a <see cref="Color"/>.
    /// </summary>
    /// <param name="value"> The value to convert. </param>
    /// <returns>
    ///     The converted <see cref="Color"/>.
    /// </returns>
    public static implicit operator Color((byte R, byte G, byte B) value)
    {
        return new(value.R, value.G, value.B);
    }

    /// <summary>
    ///     Implicitly converts the <see cref="Color"/> to an RGB tuple.
    /// </summary>
    /// <param name="value"> The value to convert. </param>
    /// <returns>
    ///     The converted tuple.
    /// </returns>
    public static implicit operator (byte R, byte G, byte B)(Color value)
    {
        return (value.R, value.G, value.B);
    }

    /// <summary>
    ///     Implicitly converts the RGB tuple to a <see cref="Color"/>.
    /// </summary>
    /// <param name="value"> The value to convert. </param>
    /// <returns>
    ///     The converted <see cref="Color"/>.
    /// </returns>
    public static implicit operator Color((float R, float G, float B) value)
    {
        return new(value.R, value.G, value.B);
    }

    /// <summary>
    ///     Implicitly converts the <see cref="Color"/> to a <see cref="System.Drawing.Color"/>.
    /// </summary>
    /// <param name="value"> The value to convert. </param>
    /// <returns>
    ///     The converted <see cref="System.Drawing.Color"/>.
    /// </returns>
    public static implicit operator System.Drawing.Color(Color value)
    {
        return System.Drawing.Color.FromArgb(value.R, value.G, value.B);
    }

    /// <summary>
    ///     Implicitly converts the  <see cref="System.Drawing.Color"/> to a <see cref="Color"/>.
    /// </summary>
    /// <param name="value"> The value to convert. </param>
    /// <returns>
    ///     The converted <see cref="Color"/>.
    /// </returns>
    public static implicit operator Color(System.Drawing.Color value)
    {
        return new(value.R, value.G, value.B);
    }

    /// <summary>
    ///     Converts the specified HSV values to a <see cref="Color"/>.
    /// </summary>
    /// <param name="h"> The hue component. </param>
    /// <param name="s"> The saturation component. </param>
    /// <param name="v"> The value component. </param>
    /// <returns>
    ///     The converted <see cref="Color"/>.
    /// </returns>
    public static Color FromHsv(float h, float s, float v)
    {
        Guard.IsBetweenOrEqualTo(h, 0, 360);
        Guard.IsBetweenOrEqualTo(s, 0, 1);
        Guard.IsBetweenOrEqualTo(v, 0, 1);

        if (s == 0)
            return (v, v, v);

        var i = MathF.Floor(h / 60) % 6;
        var f = h / 60 - MathF.Floor(h / 60);
        var p = v * (1f - s);
        var q = v * (1f - s * f);
        var t = v * (1f - s * (1f - f));
        return i switch
        {
            0 => (v, t, p),
            1 => (q, v, p),
            2 => (p, v, t),
            3 => (p, q, v),
            4 => (t, p, v),
            _ => (v, p, q)
        };
    }
}
