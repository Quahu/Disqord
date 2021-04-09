using System;

namespace Disqord
{
    public readonly partial struct Snowflake : IConvertible, IEquatable<ulong>, IEquatable<Snowflake>, IComparable<ulong>, IComparable<Snowflake>
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

        public int CompareTo(ulong other)
            => RawValue.CompareTo(other);

        public int CompareTo(Snowflake other)
            => RawValue.CompareTo(other.RawValue);

        public override bool Equals(object obj)
        {
            if (obj is Snowflake otherSnowflake)
                return RawValue == otherSnowflake.RawValue;

            if (obj is ulong otherRawValue)
                return RawValue == otherRawValue;

            return false;
        }

        public override int GetHashCode()
            => RawValue.GetHashCode();

        public override string ToString()
            => RawValue.ToString();

        public static bool TryParse(string value, out Snowflake result)
            => TryParse(value.AsSpan(), out result);

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

        public static Snowflake Parse(string value)
            => Parse(value.AsSpan());

        public static Snowflake Parse(ReadOnlySpan<char> value)
            => value.Length >= 15 && value.Length < 21
                ? ulong.Parse(value)
                : throw new FormatException();

        public static Snowflake FromDateTimeOffset(DateTimeOffset dateTimeOffset)
            => ((ulong) dateTimeOffset.ToUniversalTime().ToUnixTimeMilliseconds() - DISCORD_EPOCH) << 22;

        public static DateTimeOffset ToDateTimeOffset(ulong id)
            => DateTimeOffset.FromUnixTimeMilliseconds((long) ((id >> 22) + DISCORD_EPOCH));

    }
}
