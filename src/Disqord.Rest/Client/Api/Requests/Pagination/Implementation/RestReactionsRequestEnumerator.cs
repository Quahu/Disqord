using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    internal sealed class RestReactionsRequestEnumerator : RestRequestEnumerator<RestUser>
    {
        private readonly Snowflake _channelId;
        private readonly Snowflake _messageId;
        private readonly IEmoji _emoji;
        private readonly Snowflake? _startFromId;

        public RestReactionsRequestEnumerator(RestDiscordClient client,
            Snowflake channelId, Snowflake messageId, IEmoji emoji, int limit, Snowflake? startFromId,
            RestRequestOptions options)
            : base(client, 100, limit, options)
        {
            _channelId = channelId;
            _messageId = messageId;
            _emoji = emoji;
            _startFromId = startFromId;
        }

        protected override Task<IReadOnlyList<RestUser>> NextPageAsync(
            IReadOnlyList<RestUser> previous, RestRequestOptions options)
        {
            var amount = Remaining > 100
                ? 100
                : Remaining;
            var startFromId = _startFromId;
            if (previous != null && previous.Count > 0)
                startFromId = previous[^1].Id;

            return Client.InternalGetReactionsAsync(_channelId, _messageId, _emoji, amount, startFromId, options);
        }
    }
}
