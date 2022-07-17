using System.Collections.Generic;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway;

public static partial class GatewayEntityExtensions
{
    /// <summary>
    ///     Gets a cached channel with the given ID within this guild.
    /// </summary>
    /// <param name="guild"> The guild to get the channel within. </param>
    /// <param name="channelId"> The ID of the channel to get. </param>
    /// <returns>
    ///     The channel or <see langword="null"/> if it was not cached.
    /// </returns>
    public static CachedGuildChannel? GetChannel(this IGuild guild, Snowflake channelId)
    {
        var client = guild.GetGatewayClient();
        return client.GetChannel(guild.Id, channelId);
    }

    /// <summary>
    ///     Gets all channels within this guild.
    /// </summary>
    /// <param name="guild"> The guild to get the channels within. </param>
    /// <returns>
    ///     A dictionary of channels within this guild keyed by their IDs.
    /// </returns>
    public static IReadOnlyDictionary<Snowflake, CachedGuildChannel> GetChannels(this IGuild guild)
    {
        var client = guild.GetGatewayClient();
        if (client.CacheProvider.TryGetChannels(guild.Id, out var cache, true))
            return cache.ReadOnly();

        return ReadOnlyDictionary<Snowflake, CachedGuildChannel>.Empty;
    }

    /// <summary>
    ///     Gets a cached stage with the given ID within this guild.
    /// </summary>
    /// <param name="guild"> The guild to get the stage within. </param>
    /// <param name="stageId"> The ID of the stage to get. </param>
    /// <returns>
    ///     The stage or <see langword="null"/> if it was not cached.
    /// </returns>
    public static CachedStage? GetStage(this IGuild guild, Snowflake stageId)
    {
        var client = guild.GetGatewayClient();
        return client.GetStage(guild.Id, stageId);
    }

    /// <summary>
    ///     Gets all cached stages within this guild.
    /// </summary>
    /// <param name="guild"> The guild to get the stages within. </param>
    /// <returns>
    ///     A dictionary of stages within this guild keyed by their IDs.
    /// </returns>
    public static IReadOnlyDictionary<Snowflake, CachedStage> GetStages(this IGuild guild)
    {
        var client = guild.GetGatewayClient();
        if (client.CacheProvider.TryGetStages(guild.Id, out var cache, true))
            return cache.ReadOnly();

        return ReadOnlyDictionary<Snowflake, CachedStage>.Empty;
    }

    /// <summary>
    ///     Gets a cached guild event with the given ID within this guild.
    /// </summary>
    /// <param name="guild"> The guild to get the guild event within. </param>
    /// <param name="eventId"> The ID of the guild event to get. </param>
    /// <returns>
    ///     The guild event or <see langword="null"/> if it was not cached.
    /// </returns>
    public static CachedGuildEvent? GetEvent(this IGuild guild, Snowflake eventId)
    {
        var client = guild.GetGatewayClient();
        return client.GetGuildEvent(guild.Id, eventId);
    }

    /// <summary>
    ///     Gets all cached guild events within this guild.
    /// </summary>
    /// <param name="guild"> The guild to get the guild events within. </param>
    /// <returns>
    ///     A dictionary of guild events within this guild keyed by their IDs.
    /// </returns>
    public static IReadOnlyDictionary<Snowflake, CachedGuildEvent> GetEvents(this IGuild guild)
    {
        var client = guild.GetGatewayClient();
        if (client.CacheProvider.TryGetGuildEvents(guild.Id, out var cache, true))
            return cache.ReadOnly();

        return ReadOnlyDictionary<Snowflake, CachedGuildEvent>.Empty;
    }

    /// <summary>
    ///     Gets a cached member with the given ID within this guild.
    /// </summary>
    /// <param name="guild"> The guild to get the member within. </param>
    /// <param name="memberId"> The ID of the member to get. </param>
    /// <returns>
    ///     The member or <see langword="null"/> if it was not cached.
    /// </returns>
    public static CachedMember? GetMember(this IGuild guild, Snowflake memberId)
    {
        var client = guild.GetGatewayClient();
        return client.GetMember(guild.Id, memberId);
    }

    /// <summary>
    ///     Gets all cached members within this guild.
    /// </summary>
    /// <param name="guild"> The guild to get the members within. </param>
    /// <returns>
    ///     A dictionary of members within this guild keyed by their IDs.
    /// </returns>
    public static IReadOnlyDictionary<Snowflake, CachedMember> GetMembers(this IGuild guild)
    {
        var client = guild.GetGatewayClient();
        if (client.CacheProvider.TryGetMembers(guild.Id, out var cache, true))
            return cache.ReadOnly();

        return ReadOnlyDictionary<Snowflake, CachedMember>.Empty;
    }

    /// <summary>
    ///     Gets a cached voice state for of a member with the given ID within this guild.
    /// </summary>
    /// <param name="guild"> The guild to get the voice state within. </param>
    /// <param name="memberId"> The ID of the member to get the voice state of. </param>
    /// <returns>
    ///     The voice state or <see langword="null"/> if it was not cached.
    /// </returns>
    public static CachedVoiceState? GetVoiceState(this IGuild guild, Snowflake memberId)
    {
        var client = guild.GetGatewayClient();
        return client.GetVoiceState(guild.Id, memberId);
    }

    /// <summary>
    ///     Gets all cached voice states within this guild.
    /// </summary>
    /// <param name="guild"> The guild to get the voice states within. </param>
    /// <returns>
    ///     A dictionary of voice states within this guild keyed by IDs of their members.
    /// </returns>
    public static IReadOnlyDictionary<Snowflake, CachedVoiceState> GetVoiceStates(this IGuild guild)
    {
        var client = guild.GetGatewayClient();
        if (client.CacheProvider.TryGetVoiceStates(guild.Id, out var cache, true))
            return cache.ReadOnly();

        return ReadOnlyDictionary<Snowflake, CachedVoiceState>.Empty;
    }

    /// <summary>
    ///     Gets a cached presence of a member with the given ID within this guild.
    /// </summary>
    /// <param name="guild"> The guild to get the presence within. </param>
    /// <param name="memberId"> The ID of the member to get the presence of. </param>
    /// <returns>
    ///     The presence or <see langword="null"/> if it was not cached.
    /// </returns>
    public static CachedPresence? GetPresence(this IGuild guild, Snowflake memberId)
    {
        var client = guild.GetGatewayClient();
        return client.GetPresence(guild.Id, memberId);
    }

    /// <summary>
    ///     Gets all cached presences within this guild.
    /// </summary>
    /// <param name="guild"> The guild to get the presences within. </param>
    /// <returns>
    ///     A dictionary of presences within this guild keyed by IDs of their members.
    /// </returns>
    public static IReadOnlyDictionary<Snowflake, CachedPresence> GetPresences(this IGuild guild)
    {
        var client = guild.GetGatewayClient();
        if (client.CacheProvider.TryGetPresences(guild.Id, out var cache, true))
            return cache.ReadOnly();

        return ReadOnlyDictionary<Snowflake, CachedPresence>.Empty;
    }
}
