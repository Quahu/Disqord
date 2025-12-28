using System.Collections.Generic;

namespace Disqord.Rest;

public interface IBulkBanResponse
{
    IReadOnlyList<Snowflake> BannedUserIds { get; }

    IReadOnlyList<Snowflake> FailedUserIds { get; }
}
