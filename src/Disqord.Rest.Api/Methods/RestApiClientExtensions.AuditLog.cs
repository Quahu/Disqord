using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<AuditLogJsonModel> FetchAuditLogsAsync(this IRestApiClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var route = Format(Route.AuditLog.GetAuditLogs, guildId);
            return client.ExecuteAsync<AuditLogJsonModel>(route, null, options);
        }
    }
}
