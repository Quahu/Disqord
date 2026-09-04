using System.Collections.Generic;
using Disqord.Gateway.Default;

namespace Disqord.Gateway;

/// <summary>
///     Represents the configuration of automatic member chunking,
///     i.e., populating the member cache of guilds over the gateway as they become available.
/// </summary>
/// <remarks>
///     From Discord documentation:
///     <br/>
///     "When initially connecting, if you don't have the <c>GUILD_PRESENCES</c> Gateway Intent,
///     or if the guild is over 75k members, it will only send members who are in voice, plus the member for you (the connecting user).
///     Otherwise, if a guild has over <c>large_threshold</c> members (value in the Gateway Identify),
///     it will only send members who are online, have a role, have a nickname, or are in a voice channel,
///     and if it has under <c>large_threshold</c> members, it will send all members.
///     If a client wishes to receive additional members, they need to explicitly request them via this operation."
/// </remarks>
/// <seealso cref="IAutomaticGatewayMemberChunkingSelector"/>
/// <seealso href="https://docs.discord.com/developers/events/gateway-events#request-guild-members"> Discord documentation </seealso>
public class AutomaticMemberChunkingConfiguration
{
    /// <summary>
    ///     Gets or sets whether guilds should have their members automatically chunked upon becoming available.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see langword="false"/>.<br/>
    ///     Requires the <see cref="GatewayIntents.Members"/> intent.
    /// </remarks>
    /// <seealso href="https://docs.discord.com/developers/events/gateway-events#request-guild-members"> Discord documentation </seealso>
    public bool IsEnabled { get; set; }

    /// <summary>
    ///     Gets or sets the strategy used to chunk guilds.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see cref="AutomaticMemberChunkingStrategy.AsAvailable"/>.
    /// </remarks>
    public AutomaticMemberChunkingStrategy Strategy { get; set; } = AutomaticMemberChunkingStrategy.AsAvailable;

    /// <summary>
    ///     Gets or sets the IDs of the guilds to chunk.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see langword="null"/>, i.e. all guilds are chunked.<br/>
    ///     Only used by <see cref="DefaultAutomaticGatewayMemberChunkingSelector"/>;
    ///     to resolve the IDs dynamically, implement <see cref="IAutomaticGatewayMemberChunkingSelector"/> and register it instead.
    /// </remarks>
    /// <example>
    ///     Resolving the guilds to chunk dynamically.
    ///     <code language="csharp">
    ///     public class CustomChunkingSelector : IAutomaticMemberChunkingSelector
    ///     {
    ///         public async ValueTask&lt;IEnumerable&lt;Snowflake&gt;&gt; SelectAsync(IShard shard, IReadOnlyList&lt;Snowflake&gt; guildIds, CancellationToken cancellationToken)
    ///         {
    ///             // Pull guild IDs to chunk from database etc.
    ///         }
    ///     }
    ///
    ///     services.AddAutomaticMemberChunkingSelector&lt;CustomChunkingSelector&gt;();
    ///     </code>
    /// </example>
    public IEnumerable<Snowflake>? GuildIds { get; set; }

    /// <summary>
    ///     Gets or sets the maximum number of guilds chunked concurrently per shard.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see langword="null"/>, i.e. unlimited.
    /// </remarks>
    public int? MaxConcurrentGuilds { get; set; }
}
