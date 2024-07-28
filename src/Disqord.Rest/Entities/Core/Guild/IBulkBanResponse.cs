using System.Collections.Generic;

namespace Disqord.Rest.Entities.Core.Guild;

public interface IBulkBanResponse
{
    IReadOnlyList<Snowflake> BannedUserIds { get; }

    IReadOnlyList<Snowflake> FailedUserIds { get; }
}
