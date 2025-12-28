using System.Collections.Generic;
using Disqord.Models;

namespace Disqord.Rest;

public class TransientBulkBanResponse : TransientEntity<GuildBulkBanJsonModel>, IBulkBanResponse
{
    public IReadOnlyList<Snowflake> BannedUserIds => Model.BannedUsers;

    public IReadOnlyList<Snowflake> FailedUserIds => Model.FailedUsers;

    public TransientBulkBanResponse(GuildBulkBanJsonModel model)
        : base(model)
    { }
}
