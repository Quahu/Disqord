using System.Collections.Generic;
using Disqord.Collections;

namespace Disqord.Gateway
{
    public static partial class GatewayEntityExtensions
    {
        /// <summary>
        ///     Gets a cached channel from the specified guild.
        ///     Returns <see langword="null"/> if the channel is not cached.
        /// </summary>
        /// <param name="guild"> The guild to get the channel for. </param>
        /// <param name="channelId"> The ID of the channel to get. </param>
        /// <returns>
        ///     A cached channel from this guild.
        /// </returns>
        public static CachedGuildChannel GetChannel(this IGuild guild, Snowflake channelId)
        {
            var client = guild.GetGatewayClient();
            return client.GetChannel(guild.Id, channelId);
        }

        /// <summary>
        ///     Gets all cached channels for the specified guild.
        /// </summary>
        /// <param name="guild"> The guild to get the channels for. </param>
        /// <returns>
        ///     A dictionary of cached channels for this guild.
        /// </returns>
        public static IReadOnlyDictionary<Snowflake, CachedGuildChannel> GetChannels(this IGuild guild)
        {
            var client = guild.GetGatewayClient();
            if (client.CacheProvider.TryGetChannels(guild.Id, out var channelCache, true))
                return channelCache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedGuildChannel>.Empty;
        }

        /// <summary>
        ///     Gets a cached member from the specified guild.
        ///     Returns <see langword="null"/> if the member is not cached.
        /// </summary>
        /// <param name="guild"> The guild to get the member for. </param>
        /// <param name="channelId"> The ID of the member to get. </param>
        /// <returns>
        ///     A cached member from this guild.
        /// </returns>
        public static CachedMember GetMember(this IGuild guild, Snowflake memberId)
        {
            var client = guild.GetGatewayClient();
            return client.GetMember(guild.Id, memberId);
        }

        /// <summary>
        ///     Gets all cached members for the specified guild.
        /// </summary>
        /// <param name="guild"> The guild to get the members for. </param>
        /// <returns>
        ///     A dictionary of cached members for this guild.
        /// </returns>
        public static IReadOnlyDictionary<Snowflake, CachedMember> GetMembers(this IGuild guild)
        {
            var client = guild.GetGatewayClient();
            if (client.CacheProvider.TryGetMembers(guild.Id, out var channelCache, true))
                return channelCache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedMember>.Empty;
        }
    }
}
