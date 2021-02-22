using System;
using System.Collections.Generic;
using System.Linq;
using Disqord.Collections.Synchronized;
using Disqord.Gateway.Default;
using Microsoft.Extensions.Options;

namespace Disqord.Gateway
{
    public class DefaultGatewayCacheProvider : IGatewayCacheProvider
    {
        private readonly HashSet<Type> _supportedTypes;
        private readonly HashSet<Type> _supportedNestedTypes;
        private Dictionary<Type, object> _caches;
        private Dictionary<Type, ISynchronizedDictionary<Snowflake, object>> _nestedCaches;

        public DefaultGatewayCacheProvider(
            IOptions<DefaultGatewayCacheProviderConfiguration> options)
        {
            var configuration = options.Value;

            _supportedTypes = configuration.SupportedTypes.ToHashSet();
            _supportedNestedTypes = configuration.SupportedNestedTypes.ToHashSet();
            Reset();
        }

        public void Bind(IGatewayClient value)
        {

        }

        public bool Supports<TEntity>()
            where TEntity : CachedSnowflakeEntity
            => _supportedTypes.Contains(typeof(TEntity)) || _supportedNestedTypes.Contains(typeof(TEntity));

        public bool TryGetCache<TEntity>(out ISynchronizedDictionary<Snowflake, TEntity> cache)
            where TEntity : CachedSnowflakeEntity
        {
            if (_caches.TryGetValue(typeof(TEntity), out var boxedCache))
            {
                cache = (ISynchronizedDictionary<Snowflake, TEntity>) boxedCache;
                return true;
            }

            cache = default;
            return false;
        }

        public bool TryGetCache<TEntity>(Snowflake parentId, out ISynchronizedDictionary<Snowflake, TEntity> cache)
            where TEntity : CachedSnowflakeEntity
        {
            if (!_nestedCaches.TryGetValue(typeof(TEntity), out var nestedCache))
            {
                cache = default;
                return false;
            }

            if (nestedCache.TryGetValue(parentId, out var boxedCache))
            {
                cache = (ISynchronizedDictionary<Snowflake, TEntity>) boxedCache;
                return true;
            }

            if (_supportedNestedTypes.Contains(typeof(TEntity)))
            {
                cache = new SynchronizedDictionary<Snowflake, TEntity>();
                nestedCache.Add(parentId, cache);
                return true;
            }

            cache = default;
            return false;
        }

        public bool TryRemoveCache<TEntity>(Snowflake parentId, out ISynchronizedDictionary<Snowflake, TEntity> cache)
            where TEntity : CachedSnowflakeEntity
        {
            if (!_nestedCaches.TryGetValue(typeof(TEntity), out var nestedCache))
            {
                cache = default;
                return false;
            }

            if (nestedCache.TryRemove(parentId, out var boxedCache))
            {
                cache = (ISynchronizedDictionary<Snowflake, TEntity>) boxedCache;
                return true;
            }

            cache = default;
            return false;
        }

        public void Reset()
        {
            _caches = new(_supportedTypes.Count);
            foreach (var type in _supportedTypes)
            {
                var cacheType = typeof(SynchronizedDictionary<,>).MakeGenericType(typeof(Snowflake), type);
                var cache = Activator.CreateInstance(cacheType);
                _caches.Add(type, cache);
            }

            _nestedCaches = new(_supportedNestedTypes.Count);
            foreach (var type in _supportedNestedTypes)
            {
                var cache = new SynchronizedDictionary<Snowflake, object>();
                _nestedCaches.Add(type, cache);
            }
        }
    }
}
