using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;
using Disqord.Logging;
using Disqord.Utilities.Binding;

namespace Disqord.Gateway
{
    /// <summary>
    ///     Represents a member chunker responsible for handling requests
    ///     and responses for <see cref="GatewayPayloadOperation.RequestMembers"/>.
    /// </summary>
    public interface IGatewayChunker : IBindable<IGatewayClient>, ILogging
    {
        IGatewayClient Client { get; }

        ValueTask HandleChunkAsync(GuildMembersChunkJsonModel model);

        ValueTask<bool> ChunkAsync(IGatewayGuild guild, CancellationToken cancellationToken = default);

        ValueTask<IReadOnlyDictionary<Snowflake, IMember>> QueryAsync(Snowflake guildId, string query, int limit = 100, CancellationToken cancellationToken = default);

        ValueTask<IReadOnlyDictionary<Snowflake, IMember>> QueryAsync(Snowflake guildId, IEnumerable<Snowflake> memberIds, CancellationToken cancellationToken = default);
    }
}
