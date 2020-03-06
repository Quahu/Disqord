using System;
using System.ComponentModel;

namespace Disqord
{
    /// <summary>
    ///     Represents a color used by Discord.
    /// </summary>
    public readonly partial struct Color : IEquatable<int>, IEquatable<Color>, IComparable<int>, IComparable<Color>
    {
        /// <summary>
        ///     Gets the raw value of this <see cref="Color"/>.
        /// </summary>
        public int RawValue { get; }

        /// <summary>
        ///     Gets the red component of this <see cref="Color"/>.
        /// </summary>
        public byte R => (byte) (RawValue >> 16);

        /// <summary>
        ///     Gets the green component of this <see cref="Color"/>.
        /// </summary>
        public byte G => (byte) (RawValue >> 8);

        /// <summary>
        ///     Gets the blue component of this <see cref="Color"/>.
        /// </summary>
        public byte B => (byte) RawValue;

        /// <summary>
        ///     Initialises a new <see cref="Color"/> using the given raw value.
        /// </summary>
        /// <param name="rawValue"> The raw value. </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     "Raw value must be a non-negative value less than or equal to 16777215."
        /// </exception>
        public Color(int rawValue)
        {
            if (rawValue < 0 || rawValue > 16777215)
                throw new ArgumentOutOfRangeException(nameof(rawValue), "Raw value must be a non-negative value less than or equal to 16777215.");

            RawValue = rawValue;
        }

        /// <summary>
        ///     Initialises a new <see cref="Color"/> using the given RGB values.
        /// </summary>
        /// <param name="r"> The red (0-255) component value. </param>
        /// <param name="g"> The green (0-255) component value. </param>
        /// <param name="b"> The blue (0-255) component value. </param>
        public Color(byte r, byte g, byte b)
        {
            RawValue = (r << 16) | (g << 8) | b;
        }

        /// <summary>
        ///     Initialises a new <see cref="Color"/> using the given RGB values.
        /// </summary>
        /// <param name="r"> The red (0-1) component value. </param>
        /// <param name="g"> The green (0-1) component value. </param>
        /// <param name="b"> The blue (0-1) component value. </param>
        public Color(float r, float g, float b)
        {
            if (r < 0 || r > 1)
                throw new ArgumentOutOfRangeException(nameof(r));

            if (g < 0 || g > 1)
                throw new ArgumentOutOfRangeException(nameof(g));

            if (b < 0 || b > 1)
                throw new ArgumentOutOfRangeException(nameof(b));

            RawValue = ((byte) (r * 255) << 16) | ((byte) (g * 255) << 8) | (byte) (b * 255);
        }

        public bool Equals(Color other)
            => RawValue == other.RawValue;

        public int CompareTo(Color other)
            => RawValue.CompareTo(other.RawValue);

        public bool Equals(int other)
            => RawValue == other;

        public int CompareTo(int other)
            => RawValue.CompareTo(other);

        public override bool Equals(object obj)
            => obj is Color color && Equals(color);

        public override int GetHashCode()
            => RawValue;

        /// <summary>
        ///     Returns a hexadecimal representation of this <see cref="Color"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => $"#{RawValue:X6}";

        public static bool operator >(Color left, Color right)
            => left.RawValue > right.RawValue;

        public static bool operator <(Color left, Color right)
            => left.RawValue < right.RawValue;

        public static bool operator ==(Color left, Color right)
            => left.RawValue == right.RawValue;

        public static bool operator !=(Color left, Color right)
            => left.RawValue != right.RawValue;

        /// <summary>
        ///     Implicitly initialises a new <see cref="Color"/> from this raw value.
        /// </summary>
        /// <param name="value"> The raw value. </param>
        public static implicit operator Color(int value)
            => new Color(value);

        /// <summary>
        ///     Implicitly gets <see cref="RawValue"/> from the given <see cref="Color"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator int(Color value)
            => value.RawValue;

        public static implicit operator Color((byte R, byte G, byte B) value)
            => new Color(value.R, value.G, value.B);

        public static implicit operator (byte R, byte G, byte B)(Color value)
            => (value.R, value.G, value.B);

        public static implicit operator Color((float R, float G, float B) value)
            => new Color(value.R, value.G, value.B);

        public static implicit operator System.Drawing.Color(Color value)
            => System.Drawing.Color.FromArgb(value.R, value.G, value.B);

        public static implicit operator Color(System.Drawing.Color value)
            => new Color(value.R, value.G, value.B);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out byte r, out byte g, out byte b)
        {
            r = R;
            g = G;
            b = B;
        }
    }
}
