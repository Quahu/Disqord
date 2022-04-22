using System.Collections.Generic;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;
using Qommon.Collections.Synchronized;

namespace Disqord.Gateway
{
    public static partial class GatewayClientExtensions
    {
        /// <summary>
        ///     Gets a cached user with the given ID.
        /// </summary>
        /// <param name="client"> The client to get the user from. </param>
        /// <param name="userId"> The ID of the user to get. </param>
        /// <returns>
        ///     The user or <see langword="null"/>, if it was not cached or the user does not exist.
        /// </returns>
        public static CachedUser GetUser(this IGatewayClient client, Snowflake userId)
        {
            if (client.CacheProvider.TryGetUsers(out var cache))
                return cache.GetValueOrDefault(userId);

            return null;
        }

        /// <summary>
        ///     Gets a cached guild with the given ID.
        /// </summary>
        /// <param name="client"> The client to get the guild from. </param>
        /// <param name="guildId"> The ID of the guild to get. </param>
        /// <returns>
        ///     The user or <see langword="null"/>, if it was not cached or the bot is not in the guild.
        /// </returns>
        public static CachedGuild GetGuild(this IGatewayClient client, Snowflake guildId)
        {
            if (client.CacheProvider.TryGetGuilds(out var cache))
                return cache.GetValueOrDefault(guildId);

            return null;
        }

        // TODO: xmldocs
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

        public static CachedStage GetStage(this IGatewayClient client, Snowflake guildId, Snowflake stageId)
        {
            if (client.CacheProvider.TryGetStages(guildId, out var cache, true))
                return cache.GetValueOrDefault(stageId);

            return null;
        }

        public static IReadOnlyDictionary<Snowflake, CachedStage> GetStages(this IGatewayClient client, Snowflake guildId)
        {
            if (client.CacheProvider.TryGetStages(guildId, out var cache, true))
                return cache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedStage>.Empty;
        }

        public static CachedGuildEvent GetGuildEvent(this IGatewayClient client, Snowflake guildId, Snowflake eventId)
        {
            if (client.CacheProvider.TryGetGuildEvents(guildId, out var cache, true))
                return cache.GetValueOrDefault(eventId);

            return null;
        }

        public static IReadOnlyDictionary<Snowflake, CachedGuildEvent> GetGuildEvents(this IGatewayClient client, Snowflake guildId)
        {
            if (client.CacheProvider.TryGetGuildEvents(guildId, out var cache, true))
                return cache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedGuildEvent>.Empty;
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
