using System;
using System.Collections.Generic;
using System.Linq;
using Qommon;

namespace Disqord.Gateway.Api;

/// <summary>
///     Represents a tuple of IDs of the shards the gateway client should run
///     and the maximum identify concurrency.
/// </summary>
public readonly struct ShardSet : IEquatable<ShardSet>
{
    /// <summary>
    ///     Gets the IDs of the shards the gateway client should run.
    /// </summary>
    /// <exception cref="InvalidOperationException"> This shard set is not initialized. </exception>
    public IReadOnlyList<ShardId> ShardIds
    {
        get
        {
            if (_shardIds == null)
                Throw.InvalidOperationException("This shard set is not initialized.");

            return _shardIds;
        }
    }
    private readonly IReadOnlyList<ShardId>? _shardIds;

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
    ///     Instantiates a new <see cref="ShardSet"/> which represents the specified shards.
    /// </summary>
    /// <param name="shardIds"> The IDs of the shards. </param>
    /// <param name="maxConcurrency"> The maximum concurrency. See the property for details. </param>
    public ShardSet(IEnumerable<ShardId> shardIds, int maxConcurrency = 1)
    {
        Guard.IsGreaterThanOrEqualTo(maxConcurrency, 1);

        var list = new List<ShardId>(shardIds.TryGetNonEnumeratedCount(out var count) ? count : 8);
        using (var enumerator = shardIds.GetEnumerator())
        {
            if (!enumerator.MoveNext())
                Throw.ArgumentException("The shard set must contain at least one shard ID.", nameof(shardIds));

            do
            {
                if (!enumerator.Current.HasCount)
                    Throw.ArgumentException("The IDs of the shards must have the total shard count set.", nameof(shardIds));

                list.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
        }

        _shardIds = list.ToArray();

        MaxConcurrency = maxConcurrency;
    }

    /// <inheritdoc/>
    public bool Equals(ShardSet other)
    {
        if (_shardIds == null && other._shardIds == null)
            return true;

        if (_shardIds == null || other._shardIds == null)
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
        return _shardIds != null
            ? $"{nameof(ShardIds)}: [{string.Join(", ", _shardIds)}], {nameof(MaxConcurrency)}: {MaxConcurrency}"
            : "<invalid shard set>";
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
