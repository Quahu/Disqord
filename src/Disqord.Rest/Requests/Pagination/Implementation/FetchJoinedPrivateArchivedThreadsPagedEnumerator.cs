using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public class FetchJoinedPrivateArchivedThreadsPagedEnumerator : PagedEnumerator<IThreadChannel>
    {
        public override int PageSize => 100;

        private readonly Snowflake _channelId;
        private readonly Snowflake? _startFromId;

        public FetchJoinedPrivateArchivedThreadsPagedEnumerator(IRestClient client, Snowflake channelId, int limit, Snowflake? startFromId, IRestRequestOptions options = null)
            : base(client, limit, options)
        {
            _channelId = channelId;
            _startFromId = startFromId;
        }

        protected override async Task<IReadOnlyList<IThreadChannel>> NextPageAsync(IReadOnlyList<IThreadChannel> previousPage, IRestRequestOptions options = null)
        {
            var startFromId = _startFromId;

            if (previousPage != null && previousPage.Count > 0)
                startFromId = previousPage[^1].Id;

            var response = await Client.InternalFetchJoinedPrivateArchivedThreadsAsync(_channelId, NextAmount, startFromId, options).ConfigureAwait(false);
            if (!response.HasMore)
                Remaining = 0;

            return response.Threads;
        }
    }
}
