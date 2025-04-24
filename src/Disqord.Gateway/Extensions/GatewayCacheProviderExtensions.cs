using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Qommon.Collections.ThreadSafe;

namespace Disqord.Gateway;

[EditorBrowsable(EditorBrowsableState.Never)]
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
    public static bool TryGetUsers(this IGatewayCacheProvider cacheProvider,
        [MaybeNullWhen(false)] out IThreadSafeDictionary<Snowflake, CachedSharedUser> cache)
    {
        return cacheProvider.TryGetCache(out cache);
    }

    /// <inheritdoc cref="TryGetUsers"/>
    /// <summary>
    ///     Attempts to get the guilds cache from this cache provider.
    /// </summary>
    public static bool TryGetGuilds(this IGatewayCacheProvider cacheProvider,
        [MaybeNullWhen(false)] out IThreadSafeDictionary<Snowflake, CachedGuild> cache)
    {
        return cacheProvider.TryGetCache(out cache);
    }

    /// <inheritdoc cref="TryGetUsers"/>
    /// <summary>
    ///     Attempts to get the members cache from this cache provider.
    /// </summary>
    /// <param name="cacheProvider"> <inheritdoc cref="TryGetUsers"/> </param>
    /// <param name="guildId"> The ID of the guild to get the cache for. </param>
    /// <param name="cache"> <inheritdoc cref="TryGetUsers"/> </param>
    /// <param name="lookupOnly"> Whether the intended use of the cache is lookup-only. </param>
    public static bool TryGetMembers(this IGatewayCacheProvider cacheProvider, Snowflake guildId,
        [MaybeNullWhen(false)] out IThreadSafeDictionary<Snowflake, CachedMember> cache, bool lookupOnly = false)
    {
        return cacheProvider.TryGetCache(guildId, out cache, lookupOnly);
    }

    /// <inheritdoc cref="TryGetMembers"/>
    /// <summary>
    ///     Attempts to get the channels cache from this cache provider.
    /// </summary>
    public static bool TryGetChannels(this IGatewayCacheProvider cacheProvider, Snowflake guildId,
        [MaybeNullWhen(false)] out IThreadSafeDictionary<Snowflake, CachedGuildChannel> cache, bool lookupOnly = false)
    {
        return cacheProvider.TryGetCache(guildId, out cache, lookupOnly);
    }

    /// <inheritdoc cref="TryGetMembers"/>
    /// <summary>
    ///     Attempts to get the roles cache from this cache provider.
    /// </summary>
    public static bool TryGetRoles(this IGatewayCacheProvider cacheProvider, Snowflake guildId,
        [MaybeNullWhen(false)] out IThreadSafeDictionary<Snowflake, CachedRole> cache, bool lookupOnly = false)
    {
        return cacheProvider.TryGetCache(guildId, out cache, lookupOnly);
    }

    /// <inheritdoc cref="TryGetMembers"/>
    /// <summary>
    ///     Attempts to get the voice states cache from this cache provider.
    /// </summary>
    public static bool TryGetVoiceStates(this IGatewayCacheProvider cacheProvider, Snowflake guildId,
        [MaybeNullWhen(false)] out IThreadSafeDictionary<Snowflake, CachedVoiceState> cache, bool lookupOnly = false)
    {
        return cacheProvider.TryGetCache(guildId, out cache, lookupOnly);
    }

    /// <inheritdoc cref="TryGetMembers"/>
    /// <summary>
    ///     Attempts to get the presences cache from this cache provider.
    /// </summary>
    public static bool TryGetPresences(this IGatewayCacheProvider cacheProvider, Snowflake guildId,
        [MaybeNullWhen(false)] out IThreadSafeDictionary<Snowflake, CachedPresence> cache, bool lookupOnly = false)
    {
        return cacheProvider.TryGetCache(guildId, out cache, lookupOnly);
    }

    /// <inheritdoc cref="TryGetMembers"/>
    /// <summary>
    ///     Attempts to get the stages cache from this cache provider.
    /// </summary>
    public static bool TryGetStages(this IGatewayCacheProvider cacheProvider, Snowflake guildId,
        [MaybeNullWhen(false)] out IThreadSafeDictionary<Snowflake, CachedStage> cache, bool lookupOnly = false)
    {
        return cacheProvider.TryGetCache(guildId, out cache, lookupOnly);
    }

    /// <inheritdoc cref="TryGetMembers"/>
    /// <summary>
    ///     Attempts to get the guild events cache from this cache provider.
    /// </summary>
    public static bool TryGetGuildEvents(this IGatewayCacheProvider cacheProvider, Snowflake guildId,
        [MaybeNullWhen(false)] out IThreadSafeDictionary<Snowflake, CachedGuildEvent> cache, bool lookupOnly = false)
    {
        return cacheProvider.TryGetCache(guildId, out cache, lookupOnly);
    }

    /// <inheritdoc cref="TryGetMembers"/>
    /// <summary>
    ///     Attempts to get the messages cache from this cache provider.
    /// </summary>
    public static bool TryGetMessages(this IGatewayCacheProvider cacheProvider, Snowflake channelId,
        [MaybeNullWhen(false)] out IThreadSafeDictionary<Snowflake, CachedUserMessage> cache, bool lookupOnly = false)
    {
        return cacheProvider.TryGetCache(channelId, out cache, lookupOnly);
    }
}
