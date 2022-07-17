namespace Disqord;

public readonly partial struct Snowflake
{
    public static bool operator ==(Snowflake left, Snowflake right)
        => left.RawValue == right.RawValue;

    public static bool operator !=(Snowflake left, Snowflake right)
        => left.RawValue != right.RawValue;

    public static bool operator <(Snowflake left, Snowflake right)
        => left.RawValue < right.RawValue;

    public static bool operator <=(Snowflake left, Snowflake right)
        => left.RawValue <= right.RawValue;

    public static bool operator >(Snowflake left, Snowflake right)
        => left.RawValue > right.RawValue;

    public static bool operator >=(Snowflake left, Snowflake right)
        => left.RawValue >= right.RawValue;

    public static implicit operator Snowflake(ulong value)
        => new(value);

    public static implicit operator ulong(Snowflake value)
        => value.RawValue;
}