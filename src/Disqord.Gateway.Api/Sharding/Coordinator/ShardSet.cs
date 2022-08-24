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
    ///     Gets how many shards can identify with the gateway in parallel.
    /// </summary>
    /// <remarks>
    ///     <b>This does not limit whether individual shards can run in parallel or not.</b><br/>
    ///     It only limits whether they can <i>identify</i> in parallel and to what degree.
    ///     <para/>
    ///     This defaults to <c>1</c> for all bots and you must not
    ///     set it to a custom value unless Discord has granted
    ///     your application a higher concurrency limit.
    /// </remarks>
    /// <seealso href="https://discord.com/developers/docs/topics/gateway#sharding-for-large-bots">Discord documentation</seealso>
    public int MaxConcurrency { get; }

    /// <summary>
    ///     Gets the IDs of the shards the gateway client should run.
    /// </summary>
    public IReadOnlyList<ShardId> ShardIds { get; }

    /// <summary>
    ///     Instantiates a new <see cref="ShardSet"/> which represents the specified shards.
    /// </summary>
    /// <param name="shardIds"> The IDs of the shards. </param>
    /// <param name="maxConcurrency"> The maximum concurrency. See the property for details. </param>
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
        return $"{nameof(MaxConcurrency)}: {MaxConcurrency}, {nameof(ShardIds)}: [{string.Join(", ", ShardIds)}]";
    }

    public static bool operator ==(ShardSet left, ShardSet right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ShardSet left, ShardSet right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    ///     Creates a new <see cref="ShardSet"/> which represents shards
    ///     with indices ranging from <c>0</c> to <paramref name="count"/>.
    /// </summary>
    /// <remarks>
    ///     <b>Do not use this method for multi-process sharding.</b>
    ///     <para/>
    ///     You must instead use the constructor or other factory methods
    ///     that allow the indices to be set separately from the total shard count.
    /// </remarks>
    /// <param name="count"> The total amount of shards. </param>
    /// <param name="maxConcurrency"> The maximum concurrency. See the property for details. </param>
    public static ShardSet FromCount(int count, int maxConcurrency = 1)
    {
        var shardIds = new ShardId[count];
        for (var i = 0; i < count; i++)
        {
            shardIds[i] = new ShardId(i, count);
        }

        return new(shardIds, maxConcurrency);
    }

    /// <summary>
    ///     Creates a new <see cref="ShardSet"/> which represents shards
    ///     with indices ranging from <c>0</c> to <paramref name="count"/>,
    ///     filtered using the specified predicate.
    /// </summary>
    /// <remarks>
    ///     For multi-process sharding the predicate must yield
    ///     different results for different processes.
    /// </remarks>
    /// <example>
    ///     Running <c>16</c> shards on <c>2</c> different processes.
    ///     <code language="csharp">
    ///     // On machine 1: even indices
    ///     var shardSet = ShardSet.FromCount(16, index => index % 2 == 0);
    ///     
    ///     // On machine 2: odd indices
    ///     var shardSet = ShardSet.FromCount(16, index => index % 2 != 0);
    ///     </code>
    /// </example>
    /// <param name="count"> The total amount of shards. </param>
    /// <param name="predicate"> The predicate that filters out the shard indices. </param>
    /// <param name="maxConcurrency"> The maximum concurrency. See the property for details. </param>
    public static ShardSet FromCount(int count, Predicate<int> predicate, int maxConcurrency = 1)
    {
        var shardIds = new List<ShardId>();
        for (var i = 0; i < count; i++)
        {
            if (!predicate(i))
                continue;

            shardIds.Add(new ShardId(i, count));
        }

        return new(shardIds, maxConcurrency);
    }

    /// <summary>
    ///     Creates a new <see cref="ShardSet"/> which represents shards
    ///     from <paramref name="indices"/> with <paramref name="count"/>.
    /// </summary>
    /// <param name="indices"> The indices of the shards. </param>
    /// <param name="count"> The total amount of shards. </param>
    /// <param name="maxConcurrency"> The maximum concurrency. See the property for details. </param>
    public static ShardSet FromIndices(Range indices, int count, int maxConcurrency = 1)
    {
        var (offset, length) = indices.GetOffsetAndLength(count);
        var shardIds = new ShardId[length];
        for (var i = 0; i < length; i++)
            shardIds[i] = new ShardId(i + offset, count);

        return new(shardIds, maxConcurrency);
    }

    /// <summary>
    ///     Creates a new <see cref="ShardSet"/> which represents shards
    ///     from <paramref name="indices"/> with <paramref name="count"/>.
    /// </summary>
    /// <param name="indices"> The indices of the shards. </param>
    /// <param name="count"> The total amount of shards. </param>
    /// <param name="maxConcurrency"> The maximum concurrency. See the property for details. </param>
    public static ShardSet FromIndices(IEnumerable<int> indices, int count, int maxConcurrency = 1)
    {
        var shardIds = indices.Select(index => new ShardId(index, count));
        return new(shardIds, maxConcurrency);
    }
}
