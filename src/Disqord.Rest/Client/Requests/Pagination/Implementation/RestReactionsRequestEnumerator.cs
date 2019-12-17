using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    internal sealed class RestReactionsRequestEnumerator : RestRequestEnumerator<RestUser>
    {
        private readonly Snowflake _channelId;
        private readonly Snowflake _messageId;
        private readonly IEmoji _emoji;
        private readonly RetrievalDirection _direction;
        private readonly Snowflake? _startFromId;

        public RestReactionsRequestEnumerator(RestDiscordClient client,
            Snowflake channelId, Snowflake messageId, IEmoji emoji, int limit, RetrievalDirection direction, Snowflake? startFromId) : base(client, 100, limit)
        {
            _channelId = channelId;
            _messageId = messageId;
            _emoji = emoji;
            _direction = direction;
            _startFromId = startFromId;
        }

        protected override Task<IReadOnlyList<RestUser>> NextPageAsync(
            IReadOnlyList<RestUser> previous, RestRequestOptions options = null)
        {
            var amount = Remaining > 100
                ? 100
                : Remaining;
            var startFromId = _startFromId;
            if (previous != null && previous.Count > 0)
            {
                startFromId = _direction switch
                {
                    RetrievalDirection.Before => previous[previous.Count - 1].Id,
                    RetrievalDirection.After => previous[0].Id,
                    RetrievalDirection.Around => throw new NotImplementedException(),
                    _ => throw new ArgumentOutOfRangeException("direction"),
                };
            }

            return Client.InternalGetReactionsAsync(_channelId, _messageId, _emoji, amount, _direction, startFromId, options);
        }
    }
}
