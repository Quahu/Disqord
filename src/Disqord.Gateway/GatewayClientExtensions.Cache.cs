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

        public static CachedMember GetMember(this IGatewayClient client, Snowflake guildId, Snowflake memberId)
        {
            if (client.CacheProvider.TryGetMembers(guildId, out var cache))
                return cache.GetValueOrDefault(memberId);

            return null;
        }
    }
}
