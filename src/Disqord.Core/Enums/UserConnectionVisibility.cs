namespace Disqord;

/// <summary>
///     Represents the visibility of a user's connection.
/// </summary>
public enum UserConnectionVisibility : byte
{
    /// <summary>
    ///     The connection is invisible to everyone except the user themselves.
    /// </summary>
    None = 0,

    /// <summary>
    ///     The connection is visible to everyone.
    /// </summary>
    Everyone = 1
}