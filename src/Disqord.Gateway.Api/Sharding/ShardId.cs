using System;
using Qommon;

namespace Disqord.Gateway.Api;

/// <summary>
///     Represents the ID of a shard.
/// </summary>
public readonly struct ShardId : IEquatable<ShardId>, IComparable<ShardId>
{
    /// <summary>
    ///     Gets the index of the shard.
    /// </summary>
    public int Index { get; }

    /// <summary>
    ///     Gets the total amount of shards.
    /// </summary>
    /// <remarks>
    ///     <see cref="HasCount"/> can be used to check whether this returns a valid value.
    /// </remarks>
    /// <returns>
    ///     Returns the total amount of shards or
    ///     <c>0</c> if this instance is <see langword="default"/> or
    ///     <c>-1</c> if this instance represents just the index alone for lookup purposes.
    /// </returns>
    public int Count { get; }

    /// <summary>
    ///     Gets whether this instance returns a valid value from <see cref="Count"/>.
    /// </summary>
    public bool HasCount => Count > 0;

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

    private ShardId(int index)
    {
        Index = index;
        Count = -1;
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
    public int CompareTo(ShardId other)
    {
        if (Count == 0 && other.Count == 0)
            return 0;

        if (Count == 0 || other.Count == 0)
            return Count.CompareTo(other.Count);

        return Index.CompareTo(other.Index);
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
            ? $"Shard #{Index}"
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
    ///     Gets a <see cref="ShardId"/> from just the index alone.
    /// </summary>
    /// <remarks>
    ///     A shard ID without the total amount of shards can be used for shard lookup,
    ///     but cannot be used within a <see cref="ShardSet"/>.
    /// </remarks>
    /// <param name="index"> The index of the shard. </param>
    /// <returns>
    ///     The <see cref="ShardId"/>.
    /// </returns>
    public static ShardId FromIndex(int index)
    {
        Guard.IsNotNegative(index);

        return new(index);
    }

    /// <summary>
    ///     Gets a <see cref="ShardId"/> that would manage the guild of the given ID.
    /// </summary>
    /// <remarks>
    ///     The formula for calculating the index is:
    ///     (<paramref name="guildId"/> >> <c>22</c>) % <paramref name="count"/>.
    /// </remarks>
    /// <param name="guildId"> The ID of the guild. </param>
    /// <param name="count"> The total amount of shards. </param>
    /// <returns>
    ///     The <see cref="ShardId"/>.
    /// </returns>
    public static ShardId FromGuildId(Snowflake guildId, int count)
    {
        Guard.IsPositive(count);

        var index = (int) ((guildId >> 22) % (ulong) count);
        return new(index, count);
    }
}
