using System;

namespace Disqord
{
    public readonly struct Snowflake : IConvertible, IEquatable<ulong>, IEquatable<Snowflake>, IComparable<ulong>, IComparable<Snowflake>
    {
        public const ulong DISCORD_EPOCH = 1420070400000;

        public ulong RawValue { get; }

        public DateTimeOffset CreatedAt => ToDateTimeOffset(RawValue);

        public byte InternalWorkerId => (byte) ((RawValue & 0x3E0000) >> 17);

        public byte InternalProcessId => (byte) ((RawValue & 0x1F000) >> 12);

        public ushort Increment => (ushort) (RawValue & 0xFFF);

        public Snowflake(ulong rawValue)
        {
            RawValue = rawValue;
        }

        public bool Equals(ulong other)
            => RawValue == other;

        public bool Equals(Snowflake other)
            => RawValue == other.RawValue;

        public override bool Equals(object obj)
        {
            if (obj is Snowflake snowflake)
                return snowflake.RawValue == RawValue;

            if (obj is ulong rawValue)
                return rawValue == RawValue;

            return false;
        }

        public override int GetHashCode()
            => RawValue.GetHashCode();

        public override string ToString()
            => RawValue.ToString();

        public static bool operator ==(Snowflake left, Snowflake right)
            => left.RawValue == right.RawValue;

        public static bool operator !=(Snowflake left, Snowflake right)
            => left.RawValue != right.RawValue;

        public static bool operator <(Snowflake left, Snowflake right)
            => left.CompareTo(right) < 0;

        public static bool operator <=(Snowflake left, Snowflake right)
            => left.CompareTo(right) <= 0;

        public static bool operator >(Snowflake left, Snowflake right)
            => left.CompareTo(right) > 0;

        public static bool operator >=(Snowflake left, Snowflake right)
            => left.CompareTo(right) >= 0;

        public static bool TryParse(string value, out Snowflake result)
            => TryParse(value.AsSpan(), out result);

        public static bool TryParse(ReadOnlySpan<char> value, out Snowflake result)
        {
            if (value.Length >= 15 && value.Length <= 21 && ulong.TryParse(value, out var ulongResult))
            {
                result = ulongResult;
                return true;
            }

            result = default;
            return false;
        }

        public static Snowflake Parse(string value)
            => Parse(value.AsSpan());

        public static Snowflake Parse(ReadOnlySpan<char> value)
            => value.Length >= 15 && value.Length <= 21
                ? ulong.Parse(value)
                : throw new FormatException();

        public static Snowflake FromDateTimeOffset(DateTimeOffset dateTimeOffset)
            => ((ulong) dateTimeOffset.ToUniversalTime().ToUnixTimeMilliseconds() - DISCORD_EPOCH) << 22;

        public static DateTimeOffset ToDateTimeOffset(ulong id)
            => DateTimeOffset.FromUnixTimeMilliseconds((long) ((id >> 22) + DISCORD_EPOCH));

        public int CompareTo(ulong other)
            => RawValue.CompareTo(other);

        public int CompareTo(Snowflake other)
            => RawValue.CompareTo(other);

        TypeCode IConvertible.GetTypeCode()
            => RawValue.GetTypeCode();

        bool IConvertible.ToBoolean(IFormatProvider provider)
            => ((IConvertible) RawValue).ToBoolean(provider);

        byte IConvertible.ToByte(IFormatProvider provider)
            => ((IConvertible) RawValue).ToByte(provider);

        char IConvertible.ToChar(IFormatProvider provider)
            => ((IConvertible) RawValue).ToChar(provider);

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
            => ((IConvertible) RawValue).ToDateTime(provider);

        decimal IConvertible.ToDecimal(IFormatProvider provider)
            => ((IConvertible) RawValue).ToDecimal(provider);

        double IConvertible.ToDouble(IFormatProvider provider)
            => ((IConvertible) RawValue).ToDouble(provider);

        short IConvertible.ToInt16(IFormatProvider provider)
            => ((IConvertible) RawValue).ToInt16(provider);

        int IConvertible.ToInt32(IFormatProvider provider)
            => ((IConvertible) RawValue).ToInt32(provider);

        long IConvertible.ToInt64(IFormatProvider provider)
            => ((IConvertible) RawValue).ToInt64(provider);

        sbyte IConvertible.ToSByte(IFormatProvider provider)
            => ((IConvertible) RawValue).ToSByte(provider);

        float IConvertible.ToSingle(IFormatProvider provider)
            => ((IConvertible) RawValue).ToSingle(provider);

        string IConvertible.ToString(IFormatProvider provider)
            => RawValue.ToString(provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
            => ((IConvertible) RawValue).ToType(conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider)
            => ((IConvertible) RawValue).ToUInt16(provider);

        uint IConvertible.ToUInt32(IFormatProvider provider)
            => ((IConvertible) RawValue).ToUInt32(provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider)
            => ((IConvertible) RawValue).ToUInt64(provider);

        public static implicit operator Snowflake(ulong value)
            => new Snowflake(value);

        public static implicit operator ulong(Snowflake value)
            => value.RawValue;
    }
}
