namespace Disqord.Gateway
{
    public static partial class GatewayClientExtensions
    {
        public static CachedUser GetUser(this IGatewayClient client, Snowflake userId)
        {
            if (client.CacheProvider.TryGetUsers(out var cache))
                return cache.GetValueOrDefault(userId);

            return null;
        }

        public static CachedGuild GetGuild(this IGatewayClient client, Snowflake guildId)
        {
            if (client.CacheProvider.TryGetGuilds(out var cache))
                return cache.GetValueOrDefault(guildId);

            return null;
        }

        public static CachedMember GetMember(this IGatewayClient client, Snowflake guildId, Snowflake memberId)
        {
            if (client.CacheProvider.TryGetMembers(guildId, out var cache, true))
                return cache.GetValueOrDefault(memberId);

            return null;
        }

        public static CachedGuildChannel GetChannel(this IGatewayClient client, Snowflake guildId, Snowflake channelId)
        {
            if (client.CacheProvider.TryGetChannels(guildId, out var cache, true))
                return cache.GetValueOrDefault(channelId);

            return null;
        }

        public static CachedRole GetRole(this IGatewayClient client, Snowflake guildId, Snowflake roleId)
        {
            if (client.CacheProvider.TryGetRoles(guildId, out var cache, true))
                return cache.GetValueOrDefault(roleId);

            return null;
        }

        public static CachedUserMessage GetMessage(this IGatewayClient client, Snowflake channelId, Snowflake messageId)
        {
            if (client.CacheProvider.TryGetMessages(channelId, out var cache, true))
                return cache.GetValueOrDefault(messageId);

            return null;
        }
    }
}
