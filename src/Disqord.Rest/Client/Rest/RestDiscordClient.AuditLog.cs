//using System;
//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Linq;
//using System.Threading.Tasks;
//using Disqord.Rest.AuditLogs;

//namespace Disqord.Rest
//{
//    public partial class RestDiscordClient : IRestDiscordClient
//    {
//        public RestRequestEnumerator<RestAuditLog> GetAuditLogsEnumerator(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null)
//            => GetAuditLogsEnumerator<RestAuditLog>(guildId, limit, userId, startFromId);

//        public RestRequestEnumerator<T> GetAuditLogsEnumerator<T>(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null) where T : RestAuditLog
//        {
//            var enumerator = new RestRequestEnumerator<T>();
//            var remaining = limit;
//            do
//            {
//                var amount = remaining > 100 ? 100 : remaining;
//                remaining -= amount;
//                enumerator.Enqueue(async (previous, options) =>
//                {
//                    var startFrom = startFromId;
//                    if (previous != null && previous.Count > 0)
//                        startFrom = previous[previous.Count - 1].Id;

//                    var auditLogs = await InternalGetAuditLogsAsync<T>(guildId, amount, userId, startFrom, options).ConfigureAwait(false);
//                    if (auditLogs.Count < 100)
//                        enumerator.Cancel();

//                    return auditLogs;
//                });
//            }
//            while (remaining > 0);
//            return enumerator;
//        }

//        public Task<IReadOnlyList<RestAuditLog>> GetAuditLogsAsync(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null)
//            => GetAuditLogsAsync<RestAuditLog>(guildId, limit, userId, startFromId, options);

//        public async Task<IReadOnlyList<T>> GetAuditLogsAsync<T>(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog
//        {
//            if (limit == 0)
//                return ImmutableArray<T>.Empty;

//            if (limit <= 100)
//                return await InternalGetAuditLogsAsync<T>(guildId, limit, userId, startFromId, options).ConfigureAwait(false);

//            var enumerator = GetAuditLogsEnumerator<T>(guildId, limit, userId, startFromId);
//            await using (enumerator.ConfigureAwait(false))
//            {
//                return await enumerator.FlattenAsync(options).ConfigureAwait(false);
//            }
//        }

//        internal async Task<IReadOnlyList<T>> InternalGetAuditLogsAsync<T>(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog
//        {
//            var model = await ApiClient.GetGuildAuditLogAsync(guildId, limit, userId, GetAuditLogAction(typeof(T)), startFromId, options).ConfigureAwait(false);
//            return model.AuditLogEntries.Select(x => RestAuditLog.Create(this, model, x)).OfType<T>().ToImmutableArray();
//        }

//        private AuditLogAction? GetAuditLogAction(Type type)
//        {
//            if (type == typeof(RestAuditLog) || type == typeof(RestUnknownAuditLog))
//                return null;

//            if (type == typeof(RestGuildUpdatedAuditLog))
//                return AuditLogAction.GuildUpdated;

//            if (type == typeof(RestChannelCreatedAuditLog))
//                return AuditLogAction.ChannelCreated;

//            if (type == typeof(RestChannelUpdatedAuditLog))
//                return AuditLogAction.ChannelUpdated;

//            if (type == typeof(RestChannelDeletedAuditLog))
//                return AuditLogAction.ChannelDeleted;

//            throw new ArgumentOutOfRangeException(nameof(type));
//        }
//    }
//}
