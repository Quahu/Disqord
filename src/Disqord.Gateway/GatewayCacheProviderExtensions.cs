using Disqord.Collections.Synchronized;

namespace Disqord.Gateway
{
    public static class GatewayCacheProviderExtensions
    {
        public static bool TryGetUsers(this IGatewayCacheProvider cacheProvider, out ISynchronizedDictionary<Snowflake, CachedSharedUser> cache)
            => cacheProvider.TryGetCache(out cache);

        public static bool TryGetGuilds(this IGatewayCacheProvider cacheProvider, out ISynchronizedDictionary<Snowflake, CachedGuild> cache)
            => cacheProvider.TryGetCache(out cache);

        public static bool TryGetChannels(this IGatewayCacheProvider cacheProvider, Snowflake guildId, out ISynchronizedDictionary<Snowflake, CachedGuildChannel> cache)
            => cacheProvider.TryGetCache(guildId, out cache);

        public static bool TryGetMembers(this IGatewayCacheProvider cacheProvider, Snowflake guildId, out ISynchronizedDictionary<Snowflake, CachedMember> cache)
            => cacheProvider.TryGetCache(guildId, out cache);

        public static bool TryGetMessages(this IGatewayCacheProvider cacheProvider, Snowflake guildId, out ISynchronizedDictionary<Snowflake, CachedUserMessage> cache)
            => cacheProvider.TryGetCache(guildId, out cache);
    }
}
