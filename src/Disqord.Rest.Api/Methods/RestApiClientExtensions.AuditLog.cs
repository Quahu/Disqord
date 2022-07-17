using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<AuditLogJsonModel> FetchAuditLogsAsync(this IRestApiClient client,
        Snowflake guildId, int limit = Discord.Limits.Rest.FetchAuditLogsPageSize, Snowflake? userId = null, AuditLogActionType? type = null, Snowflake? startFromId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, object>
        {
            ["limit"] = limit
        };

        if (userId != null)
            parameters["user_id"] = userId.Value;

        if (type != null)
            parameters["action_type"] = (int) type.Value;

        if (startFromId != null)
            parameters["before"] = startFromId.Value;

        var route = Format(Route.AuditLog.GetAuditLogs, parameters, guildId);
        return client.ExecuteAsync<AuditLogJsonModel>(route, null, options, cancellationToken);
    }
}