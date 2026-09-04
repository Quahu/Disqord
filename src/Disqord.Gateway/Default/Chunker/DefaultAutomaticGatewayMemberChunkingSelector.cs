using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Microsoft.Extensions.Options;

namespace Disqord.Gateway.Default;

/// <summary>
///     Represents the default implementation of an <see cref="IAutomaticGatewayMemberChunkingSelector"/>.
///     Selects the given guilds that are in <see cref="GuildIds"/>, or all of them if it is <see langword="null"/>.
/// </summary>
public class DefaultAutomaticGatewayMemberChunkingSelector : IAutomaticGatewayMemberChunkingSelector
{
    /// <summary>
    ///     Gets or sets the IDs of the guilds to chunk;
    ///     <see langword="null"/> selects all guilds.
    /// </summary>
    public IReadOnlyList<Snowflake>? GuildIds { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="DefaultAutomaticGatewayMemberChunkingSelector"/>.
    /// </summary>
    /// <param name="options"> The chunker options. </param>
    public DefaultAutomaticGatewayMemberChunkingSelector(
        IOptions<DefaultGatewayChunkerConfiguration> options)
    {
        var configuration = options.Value;
        GuildIds = configuration.AutomaticMemberChunking.GuildIds?.ToArray();
    }

    /// <inheritdoc/>
    public ValueTask<IEnumerable<Snowflake>> SelectAsync(IShard shard, IReadOnlyList<Snowflake> guildIds, CancellationToken cancellationToken)
    {
        var configuredGuildIds = GuildIds;
        if (configuredGuildIds == null)
        {
            return new(guildIds);
        }

        var availableGuildIds = guildIds.ToHashSet();
        return new(configuredGuildIds.Where(availableGuildIds.Contains));
    }
}
