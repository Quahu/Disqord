namespace Disqord;

/// <summary>
///     Represents the type of an invite.
/// </summary>
public enum InviteType
{
    /// <summary>
    ///     An invitation to a guild.
    /// </summary>
    Guild = 0,

    /// <summary>
    ///     An invitation to a group private channel.
    /// </summary>
    Group = 1,

    /// <summary>
    ///     An invitation to add the inviter as a friend.
    /// </summary>
    Friend = 2
}
