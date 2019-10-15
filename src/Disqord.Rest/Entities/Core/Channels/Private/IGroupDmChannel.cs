using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord
{
    public interface IGroupDmChannel : IPrivateChannel, IDeletable
    {
        string IconHash { get; }

        Snowflake OwnerId { get; }

        IUser Owner { get; }

        IReadOnlyDictionary<Snowflake, IUser> Recipients { get; }

        Task LeaveAsync(RestRequestOptions options = null);

        Task IDeletable.DeleteAsync(RestRequestOptions options)
            => LeaveAsync(options);
    }
}
