using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    internal sealed class FetchReactionsPagedEnumerator : PagedEnumerator<IUser>
    {
        public override int PageSize => 100;

        private readonly Snowflake _channelId;
        private readonly Snowflake _messageId;
        private readonly LocalEmoji _emoji;
        private readonly Snowflake? _startFromId;

        public FetchReactionsPagedEnumerator(
            IRestClient client,
            Snowflake channelId, Snowflake messageId, LocalEmoji emoji, int limit, Snowflake? startFromId,
            IRestRequestOptions options)
            : base(client, limit, options)
        {
            _channelId = channelId;
            _messageId = messageId;
            _emoji = emoji;
            _startFromId = startFromId;
        }

        [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Passed via constructor, method is not exposed to the user.")]
        protected override Task<IReadOnlyList<IUser>> NextPageAsync(IReadOnlyList<IUser> previousPage, IRestRequestOptions options = null)
        {
            var startFromId = _startFromId;
            if (previousPage != null && previousPage.Count > 0)
                startFromId = previousPage[^1].Id;

            return Client.InternalFetchReactionsAsync(_channelId, _messageId, _emoji, NextAmount, startFromId, options);
        }
    }
}
