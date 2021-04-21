using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.AuditLogs;

namespace Disqord.Rest
{
    internal sealed class FetchAuditLogsPagedEnumerator<TAuditLog> : PagedEnumerator<TAuditLog>
        where TAuditLog : IAuditLog
    {
        public override int PageSize => 100;

        private readonly Snowflake _guildId;
        private readonly Snowflake? _actorId;
        private readonly Snowflake? _startFromId;

        public FetchAuditLogsPagedEnumerator(IRestClient client,
            Snowflake guildId, int limit, Snowflake? actorId, Snowflake? startFromId,
            IRestRequestOptions options)
            : base(client, limit, options)
        {
            _guildId = guildId;
            _actorId = actorId;
            _startFromId = startFromId;
        }

        protected override Task<IReadOnlyList<TAuditLog>> NextPageAsync(
            IReadOnlyList<TAuditLog> previousPage, IRestRequestOptions options = null)
        {
            var startFromId = _startFromId;
            if (previousPage != null && previousPage.Count > 0)
                startFromId = previousPage[^1].Id;

            return Client.InternalFetchAuditLogsAsync<TAuditLog>(_guildId, NextAmount, _actorId, startFromId, options);
        }
    }
}
