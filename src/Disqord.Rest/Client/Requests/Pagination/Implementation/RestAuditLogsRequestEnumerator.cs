using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest.AuditLogs;

namespace Disqord.Rest
{
    internal sealed class RestAuditLogsRequestEnumerator<T> : RestRequestEnumerator<T>
        where T : RestAuditLog
    {
        private readonly Snowflake _guildId;
        private readonly Snowflake? _userId;
        private readonly Snowflake? _startFromId;

        public RestAuditLogsRequestEnumerator(RestDiscordClient client, 
            Snowflake guildId, int limit, Snowflake? userId, Snowflake? startFromId,
            RestRequestOptions options)
            : base(client, 100, limit, options)
        {
            _guildId = guildId;
            _userId = userId;
            _startFromId = startFromId;
        }

        protected override Task<IReadOnlyList<T>> NextPageAsync(
            IReadOnlyList<T> previous, RestRequestOptions options)
        {
            var amount = Remaining > 100
                ? 100
                : Remaining;
            var startFromId = _startFromId;
            if (previous != null && previous.Count > 0)
                startFromId = previous[^1].Id;

            return Client.InternalGetAuditLogsAsync<T>(_guildId, amount, _userId, startFromId, options);
        }
    }
}
