using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    internal sealed class RestMessagesRequestEnumerator : RestRequestEnumerator<RestMessage>
    {
        private readonly Snowflake _guildId;
        private readonly RetrievalDirection _direction;
        private readonly Snowflake? _startFromId;

        public RestMessagesRequestEnumerator(RestDiscordClient client,
            Snowflake guildId, int limit, RetrievalDirection direction, Snowflake? startFromId,
            RestRequestOptions options) 
            : base(client, 100, limit, options)
        {
            _guildId = guildId;
            _direction = direction;
            _startFromId = startFromId;
        }

        protected override Task<IReadOnlyList<RestMessage>> NextPageAsync(
            IReadOnlyList<RestMessage> previous, RestRequestOptions options)
        {
            var amount = Remaining > 100
                ? 100
                : Remaining;
            var startFromId = _startFromId;
            if (previous != null && previous.Count > 0)
            {
                startFromId = _direction switch
                {
                    RetrievalDirection.Before => previous[^1].Id,
                    RetrievalDirection.After => previous[0].Id,
                    RetrievalDirection.Around => throw new NotImplementedException(),
                    _ => throw new ArgumentOutOfRangeException("direction"),
                };
            }

            return Client.InternalGetMessagesAsync(_guildId, amount, _direction, startFromId, options);
        }
    }
}
