using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var segment = new ArraySegment<Snowflake>(_messageIds, _offset, amount);
            _offset += amount;
            await (amount == 1
                ? Client.DeleteMessageAsync(_channelId, segment[0], options)
                : Client.InternalDeleteMessagesAsync(_channelId, segment, options)).ConfigureAwait(false);
            return segment;
        }
    }
}
