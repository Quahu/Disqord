using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Disqord.Gateway;
using Disqord.Gateway.Api;

namespace Disqord.Hosting;

public class DiscordClientHostingContext
{
    /// <summary>
    ///     Gets or sets the bot token.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    ///     Gets or sets the gateway intents.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see cref="GatewayIntents.LibraryRecommended"/> which are the intents the library recommends.
    ///     <inheritdoc cref="GatewayIntents.LibraryRecommended"/>
    /// </remarks>
    /// <example>
    ///     Because the recommended intent set does not include <see cref="GatewayIntents.DirectMessages"/> nor <see cref="GatewayIntents.DirectReactions"/>
    ///     the bot will not receive messages nor reactions in private (direct message) channels.<br/>
    ///     You can enable them with the following code:
    ///     <code language="csharp">
    ///     Intents |= GatewayIntents.DirectMessages | GatewayIntents.DirectReactions;
    ///     </code>
    /// </example>
    public GatewayIntents Intents { get; set; } = GatewayIntents.LibraryRecommended;

    /// <summary>
    ///     Gets or sets the mode of delaying and firing the <see cref="DiscordClientBase.Ready"/> event.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see cref="Disqord.ReadyEventDelayMode.Guilds"/>.
    /// </remarks>
    public ReadyEventDelayMode ReadyEventDelayMode { get; set; } = ReadyEventDelayMode.Guilds;

    /// <summary>
    ///     Gets or sets the custom shard set that should
    ///     be used instead of Discord's recommended amount of shards.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see langword="null"/>, i.e. no custom shard set.<para/>
    ///
    ///     <b>Do not use this if you are unfamiliar with sharding
    ///     or wish to shard on multiple machines.</b><para/>
    ///
    ///     This basically exists just for testing,
    ///     but if you <i>know</i> your bot won't grow beyond
    ///     the single shard guild limit you can set this to
    ///     prevent the library from fetching
    ///     Discord's recommended amount of shards.
    /// </remarks>
    public ShardSet? CustomShardSet { get; set; }

    /// <summary>
    ///     Gets or sets the assemblies from which to register <see cref="DiscordClientService"/> implementations.
    /// </summary>
    /// <remarks>
    ///     Defaults to a list containing the result of <see cref="Assembly.GetEntryAssembly"/>.<br/>
    ///     If <see langword="null"/> or empty, the services will have to be manually registered.
    /// </remarks>
    public IList<Assembly> ServiceAssemblies { get; set; } = new List<Assembly> { Assembly.GetEntryAssembly()! };

    /// <summary>
    ///     Gets or sets the status the bot will identify with.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see langword="null"/>, i.e. the bot will show up as <see cref="UserStatus.Online"/>.
    /// </remarks>
    public UserStatus? Status { get; set; }

    /// <summary>
    ///     Gets or sets the activities the bot will identify with.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see langword="null"/>, i.e. no activities.
    /// </remarks>
    public IEnumerable<LocalActivity>? Activities { get; set; }

    /// <summary>
    ///     Gets or sets the proxy to use for Discord's REST API.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see langword="null"/>, i.e. no proxy.
    /// </remarks>
    public IWebProxy? RestProxy { get; set; }

    /// <summary>
    ///     Gets or sets the proxy to use for Discord's Gateway API.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see langword="null"/>, i.e. no proxy.
    /// </remarks>
    public IWebProxy? GatewayProxy { get; set; }
}
