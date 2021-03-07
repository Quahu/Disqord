using Disqord.Collections.Synchronized;

namespace Disqord.Gateway
{
    public static class GatewayCacheProviderExtensions
    {
        public static bool TryGetUsers(this IGatewayCacheProvider cacheProvider, out ISynchronizedDictionary<Snowflake, CachedSharedUser> cache)
            => cacheProvider.TryGetCache(out cache);

        public static bool TryGetGuilds(this IGatewayCacheProvider cacheProvider, out ISynchronizedDictionary<Snowflake, CachedGuild> cache)
            => cacheProvider.TryGetCache(out cache);

        public static bool TryGetMembers(this IGatewayCacheProvider cacheProvider, Snowflake guildId, out ISynchronizedDictionary<Snowflake, CachedMember> cache, bool lookupOnly = false)
            => cacheProvider.TryGetCache(guildId, out cache, lookupOnly);

        public static bool TryGetChannels(this IGatewayCacheProvider cacheProvider, Snowflake guildId, out ISynchronizedDictionary<Snowflake, CachedGuildChannel> cache, bool lookupOnly = false)
            => cacheProvider.TryGetCache(guildId, out cache, lookupOnly);

        public static bool TryGetRoles(this IGatewayCacheProvider cacheProvider, Snowflake guildId, out ISynchronizedDictionary<Snowflake, CachedRole> cache, bool lookupOnly = false)
            => cacheProvider.TryGetCache(guildId, out cache, lookupOnly);

        public static bool TryGetMessages(this IGatewayCacheProvider cacheProvider, Snowflake channelId, out ISynchronizedDictionary<Snowflake, CachedUserMessage> cache, bool lookupOnly = false)
            => cacheProvider.TryGetCache(channelId, out cache, lookupOnly);
    }
}
