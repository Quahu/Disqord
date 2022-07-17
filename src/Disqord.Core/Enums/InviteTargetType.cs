namespace Disqord;

/// <summary>
///     Represents the target type of an invite.
/// </summary>
public enum InviteTargetType : byte
{
    /// <summary>
    ///     The invite targets a voice channel stream.
    /// </summary>
    Stream = 1,

    /// <summary>
    ///     The invite targets a voice channel application, e.g. <c>YouTube Together</c>.
    /// </summary>
    EmbeddedApplication = 2
}