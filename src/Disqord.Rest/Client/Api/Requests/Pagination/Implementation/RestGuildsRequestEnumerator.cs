using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    internal sealed class RestGuildsRequestEnumerator : RestRequestEnumerator<RestPartialGuild>
    {
        private readonly RetrievalDirection _direction;
        private readonly Snowflake? _startFromId;

        public RestGuildsRequestEnumerator(RestDiscordClient client,
            int limit, RetrievalDirection direction, Snowflake? startFromId,
            RestRequestOptions options) 
            : base(client, 100, limit, options)
        {
            _direction = direction;
            _startFromId = startFromId;
        }

        protected override Task<IReadOnlyList<RestPartialGuild>> NextPageAsync(
            IReadOnlyList<RestPartialGuild> previous, RestRequestOptions options)
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

            return Client.InternalGetGuildsAsync(amount, _direction, startFromId, options);
        }
    }
}
