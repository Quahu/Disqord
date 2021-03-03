using System.Collections.Generic;

namespace Disqord.Gateway
{
    /// <summary>
    ///     Represents a shared gateway user, i.e. a user object that can be shared between member entities across multiple guilds.
    ///     This improves performance as well as saves on memory.
    /// </summary>
    public interface ICachedSharedUser : IUser, IGatewayEntity
    {
        /// <summary>
        ///     Gets the set containing referenced users by this shared user.
        /// </summary>
        ISet<CachedUser> References { get; }
    }
}
