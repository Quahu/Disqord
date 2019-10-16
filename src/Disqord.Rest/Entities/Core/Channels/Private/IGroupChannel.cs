using System.Collections.Generic;

namespace Disqord
{
    public partial interface IGroupChannel : IPrivateChannel, IDeletable
    {
        string IconHash { get; }

        Snowflake OwnerId { get; }

        IReadOnlyDictionary<Snowflake, IUser> Recipients { get; }
    }
}
