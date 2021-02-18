using Disqord.Utilities.Binding;

namespace Disqord.Gateway
{
    /// <summary>
    ///     Represents a cache provider responsible for specifying what entity types get cached.
    /// </summary>
    public interface IGatewayCacheProvider : IBindable<IGatewayClient>
    {
        /// <summary>
        ///     Attempts to retrieve a top-level cache for the <typeparamref name="TEntity"/> type.
        /// </summary>
        /// <typeparam name="TEntity"> The type of the entities. </typeparam>
        /// <param name="cache"> The cache if the provider supports it. </param>
        /// <returns> <see langword="true"/>, if the provider supports caching the type. </returns>
        bool TryGetCache<TEntity>(out IGatewayCache<TEntity> cache)
            where TEntity : IGatewayEntity, ISnowflakeEntity;

        /// <summary>
        ///     Attempts to retrieve a nested cache for the <typeparamref name="TEntity"/> type.
        /// </summary>
        /// <typeparam name="TEntity"> The type of the entities. </typeparam>
        /// <param name="parentId"> The ID of the parent entity. E.g. a guild ID. </param>
        /// <param name="cache"> The cache if the provider supports it. </param>
        /// <returns> <see langword="true"/>, if the provider supports caching the type. </returns>
        bool TryGetCache<TEntity>(Snowflake parentId, out IGatewayCache<TEntity> cache)
            where TEntity : IGatewayEntity, ISnowflakeEntity;
    }
}
