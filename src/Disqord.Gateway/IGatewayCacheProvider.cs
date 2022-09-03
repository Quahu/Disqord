using System.Diagnostics.CodeAnalysis;
using Disqord.Gateway.Api;
using Qommon.Binding;
using Qommon.Collections.Synchronized;

namespace Disqord.Gateway;

/// <summary>
///     Represents a cache provider responsible for individual cache management for various entity types.
/// </summary>
public interface IGatewayCacheProvider : IBindable<IGatewayClient>
{
    /// <summary>
    ///     Checks whether the <typeparamref name="TEntity"/> type is supported.
    /// </summary>
    /// <typeparam name="TEntity"> The type of the entities. </typeparam>
    /// <returns>
    ///     <see langword="true"/>, if the provider supports caching the type.
    /// </returns>
    bool Supports<TEntity>();

    /// <summary>
    ///     Attempts to retrieve a top-level cache for the <typeparamref name="TEntity"/> type.
    /// </summary>
    /// <typeparam name="TEntity"> The type of the entities. </typeparam>
    /// <param name="cache"> The cache if the provider supports it. </param>
    /// <returns>
    ///     <see langword="true"/>, if the provider supports caching the type.
    /// </returns>
    bool TryGetCache<TEntity>([MaybeNullWhen(false)] out ISynchronizedDictionary<Snowflake, TEntity> cache);

    /// <summary>
    ///     Attempts to retrieve a nested cache for the <typeparamref name="TEntity"/> type.
    /// </summary>
    /// <typeparam name="TEntity"> The type of the entities. </typeparam>
    /// <param name="parentId"> The ID of the parent entity. E.g. a guild ID. </param>
    /// <param name="cache"> The cache if the provider supports it. </param>
    /// <param name="lookupOnly">
    ///     Whether the calling code will only lookup the cache for existing values.
    ///     This is a hint for the provider so it does not create nested caches for invalid entities.
    /// </param>
    /// <returns>
    ///     <see langword="true"/>, if the provider supports caching the type.
    /// </returns>
    bool TryGetCache<TEntity>(Snowflake parentId, [MaybeNullWhen(false)] out ISynchronizedDictionary<Snowflake, TEntity> cache, bool lookupOnly = false);

    /// <summary>
    ///     Attempts to remove a nested cache for the <typeparamref name="TEntity"/> type.
    ///     This is called most often to notify the provider a parent entity
    ///     was destroyed so the allocated memory of the nested cache can be freed up.
    /// </summary>
    /// <typeparam name="TEntity"> The type of the entities. </typeparam>
    /// <param name="parentId"> The ID of the parent entity. </param>
    /// <param name="cache"> The cache if the provider supports it and had it allocated. </param>
    /// <returns>
    ///     <see langword="true"/>, if the provider supports caching the type and had the cache allocated.
    /// </returns>
    bool TryRemoveCache<TEntity>(Snowflake parentId, [MaybeNullWhen(false)] out ISynchronizedDictionary<Snowflake, TEntity> cache);

    /// <summary>
    ///     Resets the current cache for the given shard ID.
    ///     If the <paramref name="shardId"/> is not provided the entire cache is reset instead.
    /// </summary>
    /// <param name="shardId"> The <see cref="ShardId"/> to reset the cache for. </param>
    void Reset(ShardId shardId = default);

    /// <summary>
    ///     Resets the current cache for the given guild ID.
    /// </summary>
    /// <param name="guildId"> The ID of the guild to reset the cache for. </param>
    /// <param name="guild"> The guild if it was cached. </param>
    void Reset(Snowflake guildId, out CachedGuild? guild);
}
