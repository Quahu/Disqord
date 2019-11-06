//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Disqord.Rest;
//using Disqord.Rest.AuditLogs;

//namespace Disqord
//{
//    public partial interface IRestDiscordClient : IDisposable
//    {
//        RestRequestEnumerator<RestAuditLog> GetAuditLogsEnumerator(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null);

//        RestRequestEnumerator<T> GetAuditLogsEnumerator<T>(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null) where T : RestAuditLog;

//        Task<IReadOnlyList<RestAuditLog>> GetAuditLogsAsync(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null);

//        Task<IReadOnlyList<T>> GetAuditLogsAsync<T>(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog;
//    }
//}
