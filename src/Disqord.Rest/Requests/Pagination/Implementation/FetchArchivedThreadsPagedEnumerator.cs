using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public class FetchArchivedThreadsPagedEnumerator : PagedEnumerator<IThreadChannel>
    {
        public override int PageSize => 100;
        
        private readonly Snowflake _channelId;
        private readonly DateTimeOffset? _startFromTime;
        private readonly bool _enumeratePublicThreads;

        public FetchArchivedThreadsPagedEnumerator(IRestClient client, Snowflake channelId, int limit, DateTimeOffset? startFromTime, bool enumeratePublicThreads, IRestRequestOptions options = null) 
            : base(client, limit, options)
        {
            _channelId = channelId;
            _startFromTime = startFromTime;
            _enumeratePublicThreads = enumeratePublicThreads;
        }

        protected override async Task<IReadOnlyList<IThreadChannel>> NextPageAsync(IReadOnlyList<IThreadChannel> previousPage, IRestRequestOptions options = null)
        {
            var startFromTime = _startFromTime;

            if (previousPage != null && previousPage.Count > 0)
                startFromTime = previousPage[^1].ArchiveStateChangedAt;

            var task = _enumeratePublicThreads
                ? Client.InternalFetchPublicArchivedThreadsAsync(_channelId, NextAmount, startFromTime, options)
                : Client.InternalFetchPrivateArchivedThreadsAsync(_channelId, NextAmount, startFromTime, options);
            var response = await task.ConfigureAwait(false);
                
            if (!response.HasMore)
                Remaining = 0;
            
            return response.Threads;
        }
    }
}