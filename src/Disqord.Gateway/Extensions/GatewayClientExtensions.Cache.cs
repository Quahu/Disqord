using System.Collections.Generic;
using Disqord.Collections;

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

        public static IReadOnlyDictionary<Snowflake, CachedGuild> GetGuilds(this IGatewayClient client)
        {
            if (client.CacheProvider.TryGetGuilds(out var cache))
                return cache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedGuild>.Empty;
        }


        public static CachedGuildChannel GetChannel(this IGatewayClient client, Snowflake guildId, Snowflake channelId)
        {
            if (client.CacheProvider.TryGetChannels(guildId, out var cache, true))
                return cache.GetValueOrDefault(channelId);

            return null;
        }

        public static IReadOnlyDictionary<Snowflake, CachedGuildChannel> GetChannels(this IGatewayClient client, Snowflake guildId)
        {
            if (client.CacheProvider.TryGetChannels(guildId, out var cache, true))
                return cache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedGuildChannel>.Empty;
        }

        public static CachedMember GetMember(this IGatewayClient client, Snowflake guildId, Snowflake memberId)
        {
            if (client.CacheProvider.TryGetMembers(guildId, out var cache, true))
                return cache.GetValueOrDefault(memberId);

            return null;
        }

        public static IReadOnlyDictionary<Snowflake, CachedMember> GetMembers(this IGatewayClient client, Snowflake guildId)
        {
            if (client.CacheProvider.TryGetMembers(guildId, out var cache, true))
                return cache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedMember>.Empty;
        }

        public static CachedRole GetRole(this IGatewayClient client, Snowflake guildId, Snowflake roleId)
        {
            if (client.CacheProvider.TryGetRoles(guildId, out var cache, true))
                return cache.GetValueOrDefault(roleId);

            return null;
        }

        public static IReadOnlyDictionary<Snowflake, CachedRole> GetRoles(this IGatewayClient client, Snowflake guildId)
        {
            if (client.CacheProvider.TryGetRoles(guildId, out var cache, true))
                return cache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedRole>.Empty;
        }

        public static CachedVoiceState GetVoiceState(this IGatewayClient client, Snowflake guildId, Snowflake memberId)
        {
            if (client.CacheProvider.TryGetVoiceStates(guildId, out var cache, true))
                return cache.GetValueOrDefault(memberId);

            return null;
        }

        public static IReadOnlyDictionary<Snowflake, CachedVoiceState> GetVoiceStates(this IGatewayClient client, Snowflake guildId)
        {
            if (client.CacheProvider.TryGetVoiceStates(guildId, out var cache, true))
                return cache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedVoiceState>.Empty;
        }

        public static CachedPresence GetPresence(this IGatewayClient client, Snowflake guildId, Snowflake memberId)
        {
            if (client.CacheProvider.TryGetPresences(guildId, out var cache, true))
                return cache.GetValueOrDefault(memberId);

            return null;
        }

        public static IReadOnlyDictionary<Snowflake, CachedPresence> GetPresences(this IGatewayClient client, Snowflake guildId)
        {
            if (client.CacheProvider.TryGetPresences(guildId, out var cache, true))
                return cache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedPresence>.Empty;
        }

        public static CachedUserMessage GetMessage(this IGatewayClient client, Snowflake channelId, Snowflake messageId)
        {
            if (client.CacheProvider.TryGetMessages(channelId, out var cache, true))
                return cache.GetValueOrDefault(messageId);

            return null;
        }

        public static IReadOnlyDictionary<Snowflake, CachedUserMessage> GetMessages(this IGatewayClient client, Snowflake channelId)
        {
            if (client.CacheProvider.TryGetMessages(channelId, out var cache, true))
                return cache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedUserMessage>.Empty;
        }
    }
}
