namespace Disqord.Gateway;

/// <summary>
///     Represents a shared gateway user, i.e. a user object that can be shared between member entities across multiple guilds.
///     This improves performance as well as saves on memory.
/// </summary>
public interface ICachedSharedUser : IUser, IGatewayClientEntity
{
    /// <summary>
    ///     Gets the amount of users referenced by this shared user.
    /// </summary>
    int ReferenceCount { get; }

    /// <summary>
    ///     Adds a reference to the specified user.
    /// </summary>
    /// <param name="user"> The user to add the reference to. </param>
    /// <returns>
    ///     The new reference count.
    /// </returns>
    int AddReference(CachedUser user);

    /// <summary>
    ///     Removes a reference to the specified user.
    /// </summary>
    /// <param name="user"> The user to remove the reference to. </param>
    /// <returns>
    ///     The new reference count.
    /// </returns>
    int RemoveReference(CachedUser user);
}