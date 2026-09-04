using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api;

namespace Disqord.Gateway;

/// <summary>
///     Selects the guilds to chunk and the order to chunk them in, when automatic member chunking is enabled.
/// </summary>
public interface IAutomaticGatewayMemberChunkingSelector
{
    /// <summary>
    ///     Selects which of the given guilds to chunk;
    ///     with <see cref="AutomaticMemberChunkingStrategy.WhenAllAvailable"/> the returned order is the chunk order.
    /// </summary>
    /// <param name="shard"> The shard the guilds belong to. </param>
    /// <param name="guildIds"> The IDs of all of the shard's guilds. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> wrapping the IDs of the guilds to chunk.
    /// </returns>
    ValueTask<IEnumerable<Snowflake>> SelectAsync(IShard shard, IReadOnlyList<Snowflake> guildIds, CancellationToken cancellationToken);
}
