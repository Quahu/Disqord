using System;
using System.Collections.Generic;
using System.Linq;
using Disqord.Collections.Proxied;
using Disqord.Collections.Synchronized;
using Disqord.Gateway.Api;
using Microsoft.Extensions.Options;

namespace Disqord.Gateway.Default
{
    public class DefaultGatewayCacheProvider : IGatewayCacheProvider
    {
        public int MessagesPerChannel { get; }

        private readonly HashSet<Type> _supportedTypes;
        private readonly HashSet<Type> _supportedNestedTypes;
        private Dictionary<Type, object> _caches;
        private Dictionary<Type, ISynchronizedDictionary<Snowflake, object>> _nestedCaches;

        public DefaultGatewayCacheProvider(
            IOptions<DefaultGatewayCacheProviderConfiguration> options)
        {
            var configuration = options.Value;
            MessagesPerChannel = configuration.MessagesPerChannel;
            _supportedTypes = configuration.SupportedTypes.ToHashSet();
            _supportedNestedTypes = configuration.SupportedNestedTypes.ToHashSet();
            _caches = new Dictionary<Type, object>(_supportedTypes.Count);
            _nestedCaches = new Dictionary<Type, ISynchronizedDictionary<Snowflake, object>>(_supportedNestedTypes.Count);

            Reset();
        }

        public void Bind(IGatewayClient value)
        { }

        public bool Supports<TEntity>()
            => _supportedTypes.Contains(typeof(TEntity)) || _supportedNestedTypes.Contains(typeof(TEntity));

        public bool TryGetCache<TEntity>(out ISynchronizedDictionary<Snowflake, TEntity> cache)
        {
            lock (this)
            {
                if (_caches.TryGetValue(typeof(TEntity), out var boxedCache))
                {
                    cache = (ISynchronizedDictionary<Snowflake, TEntity>) boxedCache;
                    return true;
                }

                cache = default;
                return false;
            }
        }

        public bool TryGetCache<TEntity>(Snowflake parentId, out ISynchronizedDictionary<Snowflake, TEntity> cache, bool lookupOnly = false)
        {
            lock (this)
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

                if (!lookupOnly && _supportedNestedTypes.Contains(typeof(TEntity)))
                {
                    if (typeof(TEntity) == typeof(CachedUserMessage))
                    {
                        var messageCache = new MessageDictionary(MessagesPerChannel).Synchronized();
                        cache = (ISynchronizedDictionary<Snowflake, TEntity>) messageCache;
                    }
                    else if (typeof(TEntity) == typeof(CachedMember))
                    {
                        var memberCache = new MemberDictionary(this).Synchronized();
                        cache = (ISynchronizedDictionary<Snowflake, TEntity>) memberCache;
                    }
                    else
                    {
                        cache = new SynchronizedDictionary<Snowflake, TEntity>();
                    }

                    nestedCache.Add(parentId, cache);
                    return true;
                }

                cache = default;
                return false;
            }
        }

        public bool TryRemoveCache<TEntity>(Snowflake parentId, out ISynchronizedDictionary<Snowflake, TEntity> cache)
        {
            lock (this)
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
        }

        public void Reset(ShardId shardId = default)
        {
            lock (this)
            {
                if (shardId.Count <= 1)
                {
                    _caches.Clear();
                    foreach (var type in _supportedTypes)
                    {
                        var cacheType = typeof(SynchronizedDictionary<,>).MakeGenericType(typeof(Snowflake), type);
                        var cache = Activator.CreateInstance(cacheType);
                        _caches.Add(type, cache);
                    }

                    _nestedCaches.Clear();
                    foreach (var type in _supportedNestedTypes)
                    {
                        var cache = new SynchronizedDictionary<Snowflake, object>();
                        _nestedCaches.Add(type, cache);
                    }
                }
                else
                {
                    if (_caches.GetValueOrDefault(typeof(CachedGuild)) is ISynchronizedDictionary<Snowflake, CachedGuild> guildsCache)
                    {
                        lock (guildsCache)
                        {
                            foreach (var guildId in guildsCache.Keys.Where(x => ShardId.ForGuildId(x, shardId.Count) == shardId))
                            {
                                // TODO: dynamic?
                                guildsCache.Remove(guildId);

                                if (this.TryGetChannels(guildId, out var channelCache))
                                    channelCache.Clear();

                                if (this.TryGetMembers(guildId, out var memberCache))
                                    memberCache.Clear();

                                if (this.TryGetRoles(guildId, out var roleCache))
                                    roleCache.Clear();

                                if (this.TryGetVoiceStates(guildId, out var voiceStates))
                                    voiceStates.Clear();

                                if (this.TryGetPresences(guildId, out var presences))
                                    presences.Clear();
                            }
                        }
                    }
                }
            }
        }

        // TODO: investigate race conditions
        private sealed class MemberDictionary : ProxiedDictionary<Snowflake, CachedMember>
        {
            private readonly DefaultGatewayCacheProvider _provider;

            public MemberDictionary(DefaultGatewayCacheProvider provider)
                : base(new Dictionary<Snowflake, CachedMember>())
            {
                _provider = provider;
            }

            public override bool Remove(Snowflake key)
            {
                if ((Dictionary as Dictionary<Snowflake, CachedMember>).Remove(key, out var member))
                {
                    if (member.SharedUser.RemoveReference(member) == 0 && _provider.TryGetUsers(out var cache))
                        cache.Remove(key);

                    return true;
                }

                return false;
            }

            public override void Clear()
            {
                if (_provider.TryGetUsers(out var cache))
                {
                    foreach (var member in Values)
                    {
                        if (member.SharedUser.RemoveReference(member) == 0)
                            cache.Remove(member.Id);
                    }
                }

                Dictionary.Clear();
            }
        }

        private sealed class MessageDictionary : ProxiedDictionary<Snowflake, CachedUserMessage>
        {
            private readonly int _capacity;
            private SortedList<Snowflake, CachedUserMessage> List => Dictionary as SortedList<Snowflake, CachedUserMessage>;

            public MessageDictionary(int capacity)
                : base(new SortedList<Snowflake, CachedUserMessage>(capacity))
            {
                _capacity = capacity;
            }

            public override void Add(Snowflake key, CachedUserMessage value)
            {
                if (List.Count == _capacity)
                    List.RemoveAt(0);

                base.Add(key, value);
            }
        }
    }
}
