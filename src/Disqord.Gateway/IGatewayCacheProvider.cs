using Disqord.Collections.Synchronized;
using Disqord.Gateway.Api;
using Disqord.Utilities.Binding;

namespace Disqord.Gateway
{
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
        public bool Supports<TEntity>()
            where TEntity : CachedSnowflakeEntity;

        /// <summary>
        ///     Attempts to retrieve a top-level cache for the <typeparamref name="TEntity"/> type.
        /// </summary>
        /// <typeparam name="TEntity"> The type of the entities. </typeparam>
        /// <param name="cache"> The cache if the provider supports it. </param>
        /// <returns>
        ///     <see langword="true"/>, if the provider supports caching the type.
        /// </returns>
        bool TryGetCache<TEntity>(out ISynchronizedDictionary<Snowflake, TEntity> cache)
            where TEntity : CachedSnowflakeEntity;

        /// <summary>
        ///     Attempts to retrieve a nested cache for the <typeparamref name="TEntity"/> type.
        /// </summary>
        /// <typeparam name="TEntity"> The type of the entities. </typeparam>
        /// <param name="parentId"> The ID of the parent entity. E.g. a guild ID. </param>
        /// <param name="cache"> The cache if the provider supports it. </param>
        /// <returns>
        ///     <see langword="true"/>, if the provider supports caching the type.
        /// </returns>
        bool TryGetCache<TEntity>(Snowflake parentId, out ISynchronizedDictionary<Snowflake, TEntity> cache)
            where TEntity : CachedSnowflakeEntity;

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
        bool TryRemoveCache<TEntity>(Snowflake parentId, out ISynchronizedDictionary<Snowflake, TEntity> cache)
            where TEntity : CachedSnowflakeEntity;

        /// <summary>
        ///     Resets the current cache for the given shard ID.
        ///     If the <paramref name="shardId"/> is set as <see cref="ShardId.None"/> the entire cache is reset instead.
        /// </summary>
        /// <param name="shardId"> The <see cref="ShardId"/> to reset the cache for. </param>
        void Reset(ShardId shardId = default);
    }
}
