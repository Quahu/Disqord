using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;
using Qommon;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<AuditLogJsonModel> FetchAuditLogsAsync(this IRestApiClient client,
        Snowflake guildId, int limit = Discord.Limits.Rest.FetchAuditLogsPageSize, Snowflake? userId = null, AuditLogActionType? type = null,
        FetchDirection direction = FetchDirection.Before, Snowflake? startFromId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = new Dictionary<string, object>
        {
            ["limit"] = limit
        };

        if (userId != null)
            queryParameters["user_id"] = userId.Value;

        if (type != null)
            queryParameters["action_type"] = (int) type.Value;

        switch (direction)
        {
            case FetchDirection.Before:
            {
                if (startFromId != null)
                    queryParameters["before"] = startFromId.Value;

                break;
            }
            case FetchDirection.After:
            {
                if (startFromId != null)
                    queryParameters["after"] = startFromId.Value;

                break;
            }
            default:
            {
                Throw.ArgumentOutOfRangeException(nameof(direction));
                break;
            }
        }

        var route = Format(Route.AuditLog.GetAuditLogs, queryParameters, guildId);
        return client.ExecuteAsync<AuditLogJsonModel>(route, null, options, cancellationToken);
    }
}
