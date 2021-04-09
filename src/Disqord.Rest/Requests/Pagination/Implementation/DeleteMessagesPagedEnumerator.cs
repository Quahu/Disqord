using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    internal sealed class DeleteMessagesPagedEnumerator : PagedEnumerator<Snowflake>
    {
        public override int PageSize => 100;

        private readonly Snowflake _channelId;
        private readonly Snowflake[] _messageIds;

        private int _offset;

        public DeleteMessagesPagedEnumerator(
            IRestClient client,
            Snowflake channelId, Snowflake[] messageIds,
            IRestRequestOptions options)
            : base(client, messageIds.Length, options)
        {
            _channelId = channelId;
            _messageIds = messageIds;
        }

        protected override async Task<IReadOnlyList<Snowflake>> NextPageAsync(IReadOnlyList<Snowflake> previousPage, IRestRequestOptions options = null)
        {
            var amount = NextAmount;
            _offset += amount;
            var segment = new ArraySegment<Snowflake>(_messageIds, _offset, amount);
            if (amount == 1)
            {
                var messageId = segment[0];
                await Client.DeleteMessageAsync(_channelId, messageId, options).ConfigureAwait(false);
                return segment;
            }
            else
            {
                await Client.InternalDeleteMessagesAsync(_channelId, segment, options).ConfigureAwait(false);
                return segment;
            }
        }
    }
}
