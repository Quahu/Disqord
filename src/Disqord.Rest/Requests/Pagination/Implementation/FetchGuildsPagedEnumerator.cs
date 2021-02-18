using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    internal sealed class FetchGuildsPagedEnumerator : PagedEnumerator<IPartialGuild>
    {
        public override int PageSize => 100;

        private readonly RetrievalDirection _direction;
        private readonly Snowflake? _startFromId;

        public FetchGuildsPagedEnumerator(
            IRestClient client,
            int limit, RetrievalDirection direction, Snowflake? startFromId,
            IRestRequestOptions options)
            : base(client, limit, options)
        {
            _direction = direction;
            _startFromId = startFromId;
        }

        [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Passed via constructor, method is not exposed to the user.")]
        protected override Task<IReadOnlyList<IPartialGuild>> NextPageAsync(IReadOnlyList<IPartialGuild> previousPage, IRestRequestOptions options = null)
        {
            var startFromId = _startFromId;
            if (previousPage != null && previousPage.Count > 0)
            {
                startFromId = _direction switch
                {
                    RetrievalDirection.Before => previousPage[^1].Id,
                    RetrievalDirection.After => previousPage[0].Id,
                    RetrievalDirection.Around => throw new NotImplementedException(),
                    _ => throw new ArgumentOutOfRangeException("direction"),
                };
            }

            return Client.InternalFetchGuildsAsync(NextAmount, _direction, startFromId, options);
        }
    }
}
