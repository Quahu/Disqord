using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord.Gateway.Api;

/// <summary>
///     Represents a tuple of the maximum concurrency and IDs of the shards.
/// </summary>
public readonly struct ShardSet : IEquatable<ShardSet>
{
    /// <summary>
    ///     Gets the maximum concurrency of this set.
    /// </summary>
    /// <seealso href="https://discord.com/developers/docs/topics/gateway#sharding-for-large-bots">Discord documentation</seealso>
    public int MaxConcurrency { get; }

    /// <summary>
    ///     Gets the IDs of the shards of this set.
    /// </summary>
    public IReadOnlyList<ShardId> ShardIds { get; }

    /// <summary>
    ///     Instantiates a new <see cref="ShardSet"/> which creates shards
    ///     with indices ranging from <c>0</c> to <paramref name="count"/>.
    /// </summary>
    /// <remarks>
    ///     <b>Do not use this constructor for multi-machine sharding.</b><para/>
    ///     Instead, use the other overloads as they allow the indices to be set
    ///     separately from the total shard count.
    /// </remarks>
    /// <param name="count"> The total amount of shards. </param>
    /// <param name="maxConcurrency"> The maximum concurrency. </param>
    public ShardSet(int count, int maxConcurrency = 1)
    {
        var shardIds = new ShardId[count];
        for (var i = 0; i < count; i++)
        {
            shardIds[i] = new ShardId(i, count);
        }

        ShardIds = shardIds;
        MaxConcurrency = maxConcurrency;
    }

    /// <summary>
    ///     Instantiates a new <see cref="ShardSet"/> which creates shards
    ///     from <paramref name="indices"/> with <paramref name="count"/>.
    /// </summary>
    /// <param name="indices"> The indices of the shards. </param>
    /// <param name="count"> The total amount of shards. </param>
    /// <param name="maxConcurrency"> The maximum concurrency. </param>
    public ShardSet(IEnumerable<int> indices, int count, int maxConcurrency = 1)
    {
        ShardIds = indices.Select(index => new ShardId(index, count)).ToArray();
        MaxConcurrency = maxConcurrency;
    }

    /// <summary>
    ///     Instantiates a new <see cref="ShardSet"/> with the specified shards.
    /// </summary>
    /// <param name="shardIds"> The shards that should be yielded. </param>
    /// <param name="maxConcurrency"> The maximum concurrency. </param>
    public ShardSet(IEnumerable<ShardId> shardIds, int maxConcurrency = 1)
    {
        ShardIds = shardIds.ToArray();
        MaxConcurrency = maxConcurrency;
    }

    /// <inheritdoc/>
    public bool Equals(ShardSet other)
    {
        if (ShardIds == null && other.ShardIds == null)
            return true;

        if (ShardIds == null || other.ShardIds == null)
            return false;

        return MaxConcurrency == other.MaxConcurrency && ShardIds.SequenceEqual(other.ShardIds);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is ShardSet other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(MaxConcurrency, ShardIds);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Concurrency: {MaxConcurrency} | ShardIds: [{string.Join(", ", ShardIds)}]";
    }

    public static bool operator ==(ShardSet left, ShardSet right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ShardSet left, ShardSet right)
    {
        return !(left == right);
    }
}
