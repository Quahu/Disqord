using System;
using Qommon;

namespace Disqord.Gateway.Api;

public readonly struct ShardId : IEquatable<ShardId>
{
    /// <summary>
    ///     Gets the zero-based shard ID.
    /// </summary>
    public int Index { get; }

    /// <summary>
    ///     Gets the total count of shards.
    /// </summary>
    public int Count { get; }

    public ShardId(int index, int count)
    {
        Guard.IsNotNegative(index);
        Guard.IsLessThan(index, count);

        Index = index;
        Count = count;
    }

    public bool Equals(ShardId other)
    {
        if (Count == 0 && other.Count == 0)
            return true;

        if (Count == 0 || other.Count == 0)
            return false;

        return Index == other.Index;
    }

    public override int GetHashCode()
    {
        if (Count == 0)
            return -1;

        return Index.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is ShardId other && Equals(other);
    }

    public override string ToString()
    {
        return Count != 0
            ? $"Shard {Index + 1} (#{Index}) of {Count}"
            : "<invalid shard ID>";
    }

    public static bool operator ==(ShardId left, ShardId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ShardId left, ShardId right)
    {
        return !left.Equals(right);
    }

    public static ShardId FromGuildId(Snowflake guildId, int count)
    {
        Guard.IsPositive(count);

        var id = (int) ((guildId >> 22) % (ulong) count);
        return new(id, count);
    }
}
