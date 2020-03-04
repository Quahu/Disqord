using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest.AuditLogs;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        RestRequestEnumerable<RestAuditLog> GetAuditLogsEnumerable(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null);

        RestRequestEnumerable<T> GetAuditLogsEnumerable<T>(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog;

        Task<IReadOnlyList<RestAuditLog>> GetAuditLogsAsync(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null);

        Task<IReadOnlyList<T>> GetAuditLogsAsync<T>(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog;
    }
}
