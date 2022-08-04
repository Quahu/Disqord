using System;
using Qommon;

namespace Disqord.Gateway.Api;

/// <summary>
///     Represents an ID of a shard.
/// </summary>
public readonly struct ShardId : IEquatable<ShardId>
{
    /// <summary>
    ///     Gets the zero-based index of the shard.
    /// </summary>
    public int Index { get; }

    /// <summary>
    ///     Gets the total amount of shards.
    /// </summary>
    public int Count { get; }

    /// <summary>
    ///     Instantiates a new <see cref="ShardId"/>.
    /// </summary>
    /// <param name="index"> The zero-based index of the shard. </param>
    /// <param name="count"> The total amount of shards. </param>
    public ShardId(int index, int count)
    {
        Guard.IsNotNegative(index);
        Guard.IsLessThan(index, count);

        Index = index;
        Count = count;
    }

    /// <inheritdoc/>
    public bool Equals(ShardId other)
    {
        if (Count == 0 && other.Count == 0)
            return true;

        if (Count == 0 || other.Count == 0)
            return false;

        return Index == other.Index;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        if (Count == 0)
            return -1;

        return Index.GetHashCode();
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is ShardId other && Equals(other);
    }

    /// <inheritdoc/>
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

    /// <summary>
    ///     Gets a <see cref="ShardId"/> that would manage the guild of the given ID.
    /// </summary>
    /// <remarks>
    ///     The formula is: (<paramref name="guildId"/> >> <c>22</c>) % <paramref name="count"/>.
    /// </remarks>
    /// <param name="guildId"> The ID of the guild. </param>
    /// <param name="count"> The total amount of shards. </param>
    /// <returns></returns>
    public static ShardId FromGuildId(Snowflake guildId, int count)
    {
        Guard.IsPositive(count);

        var id = (int) ((guildId >> 22) % (ulong) count);
        return new(id, count);
    }
}
