using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    internal sealed class FetchMembersPagedEnumerator : PagedEnumerator<IMember>
    {
        public override int PageSize => 1000;

        private readonly Snowflake _guildId;
        private readonly Snowflake? _startFromId;

        public FetchMembersPagedEnumerator(
            IRestClient client,
            Snowflake guildId, int limit, Snowflake? startFromId,
            IRestRequestOptions options)
            : base(client, limit, options)
        {
            _guildId = guildId;
            _startFromId = startFromId;
        }

        protected override Task<IReadOnlyList<IMember>> NextPageAsync(IReadOnlyList<IMember> previousPage, IRestRequestOptions options = null)
        {
            var startFromId = _startFromId;
            if (previousPage != null && previousPage.Count > 0)
                startFromId = previousPage[^1].Id;

            return Client.InternalFetchMembersAsync(_guildId, NextAmount, startFromId, options);
        }
    }
}
