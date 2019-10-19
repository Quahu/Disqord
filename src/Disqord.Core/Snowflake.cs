﻿using System;

namespace Disqord
{
    public readonly struct Snowflake : IEquatable<ulong>, IEquatable<DateTimeOffset>, IEquatable<Snowflake>,
        IComparable<ulong>, IComparable<DateTimeOffset>, IComparable<Snowflake>
    {
        public const ulong DISCORD_EPOCH = 1420070400000;

        public const ulong MIN_VALUE = 1000000000000000;

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

        public bool Equals(DateTimeOffset other)
            => CreatedAt == other;

        public bool Equals(Snowflake other)
            => RawValue == other.RawValue;

        public override bool Equals(object obj)
            => obj is Snowflake snowflake && Equals(snowflake) || obj is ulong rawValue && Equals(rawValue);

        public override int GetHashCode()
            => RawValue.GetHashCode();

        public override string ToString()
            => RawValue.ToString();

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

        public static Snowflake FromDateTimeOffset(DateTimeOffset dateTimeOffset)
            => ((ulong) dateTimeOffset.ToUniversalTime().ToUnixTimeMilliseconds() - DISCORD_EPOCH) << 22;

        public static DateTimeOffset ToDateTimeOffset(ulong id)
            => DateTimeOffset.FromUnixTimeMilliseconds((long) ((id >> 22) + DISCORD_EPOCH));

        public int CompareTo(ulong other)
            => RawValue.CompareTo(other);

        public int CompareTo(DateTimeOffset other)
            => CreatedAt.CompareTo(other);

        public int CompareTo(Snowflake other)
            => RawValue.CompareTo(other);

        public static implicit operator Snowflake(ulong value)
            => new Snowflake(value);

        public static implicit operator ulong(Snowflake value)
            => value.RawValue;

        public static implicit operator DateTimeOffset(Snowflake value)
            => value.CreatedAt;
    }
}
