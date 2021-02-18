using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections.Synchronized;
using Microsoft.Extensions.Options;

namespace Disqord.Gateway.Default
{
    public class DefaultGatewayCacheProvider : IGatewayCacheProvider
    {
        public int MessagesPerChannel { get; }

        private readonly Dictionary<Type, object> _caches;
        private readonly Dictionary<Type, SynchronizedDictionary<Snowflake, object>> _nestedCaches;

        public DefaultGatewayCacheProvider(
            IOptions<DefaultGatewayCacheProviderConfiguration> options)
        {
            var configuration = options.Value;
            MessagesPerChannel = configuration.MessagesPerChannel;

            _caches = new Dictionary<Type, object>
            {
                [typeof(CachedSharedUser)] = new Cache<CachedSharedUser>(),
                [typeof(CachedGuild)] = new Cache<CachedGuild>(),
            };
            _nestedCaches = new Dictionary<Type, SynchronizedDictionary<Snowflake, object>>
            {
                [typeof(CachedMember)] = new SynchronizedDictionary<Snowflake, object>(),
                //[typeof(CachedMessage)] = new SynchronizedDictionary<Snowflake, object>()
            };
        }

        public void Bind(IGatewayClient value)
        {
            // TODO: bind and check against the entities' clients?
        }

        public bool TryGetCache<TEntity>(out IGatewayCache<TEntity> cache)
            where TEntity : IGatewayEntity, ISnowflakeEntity
        {
            if (_caches.TryGetValue(typeof(TEntity), out var boxedCache))
            {
                cache = (IGatewayCache<TEntity>) boxedCache;
                return true;
            }

            cache = default;
            return false;
        }

        public bool TryGetCache<TEntity>(Snowflake id, out IGatewayCache<TEntity> cache)
            where TEntity : IGatewayEntity, ISnowflakeEntity
        {
            if (_nestedCaches.TryGetValue(typeof(TEntity), out var caches))
            {
                cache = (IGatewayCache<TEntity>) caches.GetOrAdd(id, (id, @this) =>
                {
                    if (typeof(IMessage).IsAssignableFrom(typeof(TEntity)))
                        return new MessageCache<TEntity>(id, @this, @this.MessagesPerChannel);

                    return new NestedCache<TEntity>(id, @this);
                }, this);
                return true;
            }

            cache = default;
            return false;
        }

        private class Cache<TEntity> : IGatewayCache<TEntity>
            where TEntity : IGatewayEntity, ISnowflakeEntity
        {
            protected readonly IDictionary<Snowflake, TEntity> _cache;

            public Cache()
                : this(new SynchronizedDictionary<Snowflake, TEntity>())
            { }

            protected Cache(IDictionary<Snowflake, TEntity> cache)
            {
                _cache = cache;
            }

            public ValueTask<TEntity> GetAsync(Snowflake id)
            {
                _cache.TryGetValue(id, out var entity);
                return new(entity);
            }

            public ValueTask<IEnumerable<KeyValuePair<Snowflake, TEntity>>> GetRangeAsync(IEnumerable<Snowflake> ids)
            {
                if (ids == null)
                    throw new ArgumentNullException(nameof(ids));

                var entities = new Dictionary<Snowflake, TEntity>();
                lock (_cache)
                {
                    foreach (var id in ids)
                    {
                        if (_cache.TryGetValue(id, out var entity))
                            entities.Add(id, entity);
                    }
                }

                return new(entities);
            }

            public virtual ValueTask AddAsync(TEntity entity)
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _cache.Add(entity.Id, entity);
                return default;
            }

            public virtual ValueTask AddRangeAsync(IEnumerable<TEntity> entities)
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));

                lock (_cache)
                {
                    foreach (var entity in entities)
                    {
                        if (entity == null)
                            throw new ArgumentException("The entities enumerable must not contain null entries.", nameof(entities));

                        //if (_cache.ContainsKey(entity.Id))
                        //    throw new ArgumentException($"The entities enumerable contains an already added entity ({entity.Id}).", nameof(entities));

                        _cache[entity.Id] = entity;
                    }
                }

                return default;
            }

            public ValueTask<TEntity> RemoveAsync(Snowflake id)
            {
                TEntity entity;
                if (_cache is SynchronizedDictionary<Snowflake, TEntity> cacheAsDictionary)
                    cacheAsDictionary.TryRemove(id, out entity);
                else
                    _cache.Remove(id, out entity);

                return new(entity);
            }

            public ValueTask<IEnumerable<KeyValuePair<Snowflake, TEntity>>> RemoveRangeAsync(IEnumerable<Snowflake> ids)
            {
                if (ids == null)
                    throw new ArgumentNullException(nameof(ids));

                var entities = new Dictionary<Snowflake, TEntity>();
                lock (_cache)
                {
                    foreach (var id in ids)
                    {
                        TEntity entity;
                        if (_cache is SynchronizedDictionary<Snowflake, TEntity> cacheAsDictionary)
                        {
                            cacheAsDictionary.TryRemove(id, out entity);
                        }
                        else
                        {
                            _cache.Remove(id, out entity);
                        }

                        if (entity != null)
                            entities.Add(id, entity);
                    }
                }

                return new(entities);
            }

            public virtual ValueTask<IEnumerable<KeyValuePair<Snowflake, TEntity>>> ClearAsync()
            {
                lock (_cache)
                {
                    KeyValuePair<Snowflake, TEntity>[] entities;
                    if (_cache is SynchronizedDictionary<Snowflake, TEntity> cacheAsDictionary)
                    {
                        entities = cacheAsDictionary.ToArray();
                        cacheAsDictionary.Clear();
                        cacheAsDictionary.TrimExcess();
                    }
                    else
                    {
                        entities = _cache.ToArray();
                        _cache.Clear();
                    }

                    return new(entities);
                }
            }
        }

        private class NestedCache<TEntity> : Cache<TEntity>
            where TEntity : IGatewayEntity, ISnowflakeEntity
        {
            private readonly Snowflake _id;
            private readonly DefaultGatewayCacheProvider _provider;

            public NestedCache(Snowflake id, DefaultGatewayCacheProvider provider)
            {
                _id = id;
                _provider = provider;
            }

            protected NestedCache(Snowflake id, DefaultGatewayCacheProvider provider, IDictionary<Snowflake, TEntity> cache)
                : base(cache)
            {
                _id = id;
                _provider = provider;
            }

            public override ValueTask<IEnumerable<KeyValuePair<Snowflake, TEntity>>> ClearAsync()
            {
                var result = base.ClearAsync();
                _provider._nestedCaches[typeof(TEntity)].Remove(_id);
                return result;
            }
        }

        private class MessageCache<TEntity> : NestedCache<TEntity>
            where TEntity : IGatewayEntity, ISnowflakeEntity
        {
            private readonly int _capacity;

            public MessageCache(Snowflake id, DefaultGatewayCacheProvider provider, int capacity)
                : base(id, provider, new SortedList<Snowflake, TEntity>(capacity))
            {
                _capacity = capacity;
            }

            public override ValueTask AddAsync(TEntity entity)
            {
                lock (_cache)
                {
                    if (_cache.Count + 1 > _capacity)
                        (_cache as SortedList<Snowflake, TEntity>).RemoveAt(0);

                    return base.AddAsync(entity);
                }
            }

            public override ValueTask AddRangeAsync(IEnumerable<TEntity> entities)
            {
                // TODO: tidy up
                lock (_cache)
                {
                    var count = entities.Count();
                    if (_cache.Count + count > _capacity)
                    {
                        var diff = _cache.Count + count - _capacity;
                        while (diff-- > 0)
                            (_cache as SortedList<Snowflake, TEntity>).RemoveAt(0);
                    }

                    return base.AddRangeAsync(entities);
                }
            }
        }
    }
}
