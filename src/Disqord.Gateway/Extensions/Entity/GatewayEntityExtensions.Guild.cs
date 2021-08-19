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
            if (client.CacheProvider.TryGetChannels(guild.Id, out var cache, true))
                return cache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedGuildChannel>.Empty;
        }

        /// <summary>
        ///     Gets a cached stage from the specified guild.
        ///     Returns <see langword="null"/> if the stage is not cached.
        /// </summary>
        /// <param name="guild"> The guild to get the stage for. </param>
        /// <param name="stageId"> The ID of the stage to get. </param>
        /// <returns>
        ///     A cached stage from this guild.
        /// </returns>
        public static CachedStage GetStage(this IGuild guild, Snowflake stageId)
        {
            var client = guild.GetGatewayClient();
            return client.GetStage(guild.Id, stageId);
        }

        /// <summary>
        ///     Gets all cached stages for the specified guild.
        /// </summary>
        /// <param name="guild"> The guild to get the stages for. </param>
        /// <returns>
        ///     A dictionary of cached stages for this guild keyed by <see cref="IStage.Id"/>.
        /// </returns>
        public static IReadOnlyDictionary<Snowflake, CachedStage> GetStages(this IGuild guild)
        {
            var client = guild.GetGatewayClient();
            if (client.CacheProvider.TryGetStages(guild.Id, out var cache, true))
                return cache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedStage>.Empty;
        }

        /// <summary>
        ///     Gets a cached member from the specified guild.
        ///     Returns <see langword="null"/> if the member is not cached.
        /// </summary>
        /// <param name="guild"> The guild to get the member for. </param>
        /// <param name="memberId"> The ID of the member to get. </param>
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
            if (client.CacheProvider.TryGetMembers(guild.Id, out var cache, true))
                return cache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedMember>.Empty;
        }

        /// <summary>
        ///     Gets a cached voice state from the specified guild.
        ///     Returns <see langword="null"/> if the voice state is not cached.
        /// </summary>
        /// <param name="guild"> The guild to get the voice state for. </param>
        /// <param name="memberId"> The ID of the member to get the voice state for. </param>
        /// <returns>
        ///     A cached voice state from this guild.
        /// </returns>
        public static CachedVoiceState GetVoiceState(this IGuild guild, Snowflake memberId)
        {
            var client = guild.GetGatewayClient();
            return client.GetVoiceState(guild.Id, memberId);
        }

        /// <summary>
        ///     Gets all cached voice states for the specified guild.
        /// </summary>
        /// <param name="guild"> The guild to get the voice states for. </param>
        /// <returns>
        ///     A dictionary of cached voice states for this guild keyed by <see cref="IVoiceState.MemberId"/>.
        /// </returns>
        public static IReadOnlyDictionary<Snowflake, CachedVoiceState> GetVoiceStates(this IGuild guild)
        {
            var client = guild.GetGatewayClient();
            if (client.CacheProvider.TryGetVoiceStates(guild.Id, out var cache, true))
                return cache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedVoiceState>.Empty;
        }

        /// <summary>
        ///     Gets a cached presence from the specified guild.
        ///     Returns <see langword="null"/> if the presence is not cached.
        /// </summary>
        /// <param name="guild"> The guild to get the presence for. </param>
        /// <param name="memberId"> The ID of the member to get the presence for. </param>
        /// <returns>
        ///     A cached presence from this guild.
        /// </returns>
        public static CachedPresence GetPresence(this IGuild guild, Snowflake memberId)
        {
            var client = guild.GetGatewayClient();
            return client.GetPresence(guild.Id, memberId);
        }

        /// <summary>
        ///     Gets all cached presences for the specified guild.
        /// </summary>
        /// <param name="guild"> The guild to get the presences for. </param>
        /// <returns>
        ///     A dictionary of cached presences for this guild keyed by <see cref="IPresence.MemberId"/>.
        /// </returns>
        public static IReadOnlyDictionary<Snowflake, CachedPresence> GetPresences(this IGuild guild)
        {
            var client = guild.GetGatewayClient();
            if (client.CacheProvider.TryGetPresences(guild.Id, out var cache, true))
                return cache.ReadOnly();

            return ReadOnlyDictionary<Snowflake, CachedPresence>.Empty;
        }
    }
}
