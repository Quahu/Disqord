using System.Collections.Generic;
using System.Linq;

namespace Disqord.Rest;

public class CombinedBulkBanResponse : IBulkBanResponse
{
    public IReadOnlyList<Snowflake> BannedUserIds { get; }
    public IReadOnlyList<Snowflake> FailedUserIds { get; }

    public CombinedBulkBanResponse(IEnumerable<IBulkBanResponse> responses)
    {
        BannedUserIds = responses.SelectMany(response => response.BannedUserIds).ToArray();
        FailedUserIds = responses.SelectMany(response => response.FailedUserIds).ToArray();
    }
}
