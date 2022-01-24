using Qommon.Collections.Synchronized;

namespace Disqord.Gateway
{
    public static class GatewayCacheProviderExtensions
    {
        /// <summary>
        ///     Attempts to get the users cache from this cache provider.
        /// </summary>
        /// <param name="cacheProvider"> The cache provider to retrieve the cache from. </param>
        /// <param name="cache"> The given cache. </param>
        /// <returns>
        ///     <see langword="true"/> if the cache exists.
        /// </returns>
        public static bool TryGetUsers(this IGatewayCacheProvider cacheProvider, out ISynchronizedDictionary<Snowflake, CachedSharedUser> cache)
            => cacheProvider.TryGetCache(out cache);

        /// <inheritdoc cref="TryGetUsers"/>
        /// <summary>
        ///     Attempts to get the guilds cache from this cache provider.
        /// </summary>
        public static bool TryGetGuilds(this IGatewayCacheProvider cacheProvider, out ISynchronizedDictionary<Snowflake, CachedGuild> cache)
            => cacheProvider.TryGetCache(out cache);

        /// <inheritdoc cref="TryGetUsers"/>
        /// <summary>
        ///     Attempts to get the members cache from this cache provider.
        /// </summary>
        public static bool TryGetMembers(this IGatewayCacheProvider cacheProvider, Snowflake guildId, out ISynchronizedDictionary<Snowflake, CachedMember> cache, bool lookupOnly = false)
            => cacheProvider.TryGetCache(guildId, out cache, lookupOnly);

        /// <inheritdoc cref="TryGetUsers"/>
        /// <summary>
        ///     Attempts to get the channels cache from this cache provider.
        /// </summary>
        public static bool TryGetChannels(this IGatewayCacheProvider cacheProvider, Snowflake guildId, out ISynchronizedDictionary<Snowflake, CachedGuildChannel> cache, bool lookupOnly = false)
            => cacheProvider.TryGetCache(guildId, out cache, lookupOnly);

        /// <inheritdoc cref="TryGetUsers"/>
        /// <summary>
        ///     Attempts to get the roles cache from this cache provider.
        /// </summary>
        public static bool TryGetRoles(this IGatewayCacheProvider cacheProvider, Snowflake guildId, out ISynchronizedDictionary<Snowflake, CachedRole> cache, bool lookupOnly = false)
            => cacheProvider.TryGetCache(guildId, out cache, lookupOnly);

        /// <inheritdoc cref="TryGetUsers"/>
        /// <summary>
        ///     Attempts to get the voice states cache from this cache provider.
        /// </summary>
        public static bool TryGetVoiceStates(this IGatewayCacheProvider cacheProvider, Snowflake guildId, out ISynchronizedDictionary<Snowflake, CachedVoiceState> cache, bool lookupOnly = false)
            => cacheProvider.TryGetCache(guildId, out cache, lookupOnly);

        /// <inheritdoc cref="TryGetUsers"/>
        /// <summary>
        ///     Attempts to get the presences cache from this cache provider.
        /// </summary>
        public static bool TryGetPresences(this IGatewayCacheProvider cacheProvider, Snowflake guildId, out ISynchronizedDictionary<Snowflake, CachedPresence> cache, bool lookupOnly = false)
            => cacheProvider.TryGetCache(guildId, out cache, lookupOnly);

        /// <inheritdoc cref="TryGetUsers"/>
        /// <summary>
        ///     Attempts to get the stages cache from this cache provider.
        /// </summary>
        public static bool TryGetStages(this IGatewayCacheProvider cacheProvider, Snowflake guildId, out ISynchronizedDictionary<Snowflake, CachedStage> cache, bool lookupOnly = false)
            => cacheProvider.TryGetCache(guildId, out cache, lookupOnly);

        /// <inheritdoc cref="TryGetUsers"/>
        /// <summary>
        ///     Attempts to get the guild events cache from this cache provider.
        /// </summary>
        public static bool TryGetGuildEvents(this IGatewayCacheProvider cacheProvider, Snowflake guildId, out ISynchronizedDictionary<Snowflake, CachedGuildEvent> cache, bool lookupOnly = false)
            => cacheProvider.TryGetCache(guildId, out cache, lookupOnly);

        /// <inheritdoc cref="TryGetUsers"/>
        /// <summary>
        ///     Attempts to get the messages cache from this cache provider.
        /// </summary>
        public static bool TryGetMessages(this IGatewayCacheProvider cacheProvider, Snowflake channelId, out ISynchronizedDictionary<Snowflake, CachedUserMessage> cache, bool lookupOnly = false)
            => cacheProvider.TryGetCache(channelId, out cache, lookupOnly);
    }
}
