using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    internal sealed class FetchReactionsPagedEnumerator : PagedEnumerator<IUser>
    {
        public override int PageSize => 100;

        private readonly Snowflake _channelId;
        private readonly Snowflake _messageId;
        private readonly IEmoji _emoji;
        private readonly RetrievalDirection _direction;
        private readonly Snowflake? _startFromId;

        public FetchReactionsPagedEnumerator(
            IRestClient client,
            Snowflake channelId, Snowflake messageId, IEmoji emoji, int limit, RetrievalDirection direction, Snowflake? startFromId,
            IRestRequestOptions options)
            : base(client, limit, options)
        {
            _channelId = channelId;
            _messageId = messageId;
            _emoji = emoji;
            _direction = direction;
            _startFromId = startFromId;
        }

        [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Passed via constructor, method is not exposed to the user.")]
        protected override Task<IReadOnlyList<IUser>> NextPageAsync(IReadOnlyList<IUser> previousPage, IRestRequestOptions options = null)
        {
            var startFromId = _startFromId;
            if (previousPage != null && previousPage.Count > 0)
            {
                startFromId = _direction switch
                {
                    RetrievalDirection.Before => previousPage[^1].Id,
                    RetrievalDirection.After => previousPage[0].Id,
                    _ => throw new ArgumentOutOfRangeException("direction"),
                };
            }

            return Client.InternalFetchReactionsAsync(_channelId, _messageId, _emoji, NextAmount, _direction, startFromId, options);
        }
    }
}
