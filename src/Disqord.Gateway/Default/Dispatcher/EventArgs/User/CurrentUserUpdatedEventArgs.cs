using System;

namespace Disqord.Gateway;

public class CurrentUserUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the current user in the state before the update occurred.
    /// </summary>
    public CachedCurrentUser? OldCurrentUser { get; }

    /// <summary>
    ///     Gets the updated current user.
    /// </summary>
    public CachedCurrentUser NewCurrentUser { get; }

    public CurrentUserUpdatedEventArgs(
        CachedCurrentUser? oldCurrentUser,
        CachedCurrentUser newCurrentUser)
    {
        OldCurrentUser = oldCurrentUser;
        NewCurrentUser = newCurrentUser;
    }
}
