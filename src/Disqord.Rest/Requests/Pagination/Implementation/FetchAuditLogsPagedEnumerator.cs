using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.AuditLogs;

namespace Disqord.Rest;

public class FetchAuditLogsPagedEnumerator<TAuditLog> : PagedEnumerator<TAuditLog>
    where TAuditLog : IAuditLog
{
    public override int PageSize => Discord.Limits.Rest.FetchAuditLogsPageSize;

    private readonly Snowflake _guildId;
    private readonly Snowflake? _actorId;
    private readonly Snowflake? _startFromId;

    public FetchAuditLogsPagedEnumerator(IRestClient client,
        Snowflake guildId, int limit, Snowflake? actorId, Snowflake? startFromId,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, limit, options, cancellationToken)
    {
        _guildId = guildId;
        _actorId = actorId;
        _startFromId = startFromId;
    }

    protected override Task<IReadOnlyList<TAuditLog>> NextPageAsync(
        IReadOnlyList<TAuditLog>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var startFromId = _startFromId;
        if (previousPage != null && previousPage.Count > 0)
            startFromId = previousPage[^1].Id;

        return Client.InternalFetchAuditLogsAsync<TAuditLog>(_guildId, NextPageSize, _actorId, startFromId, options, cancellationToken);
    }
}
