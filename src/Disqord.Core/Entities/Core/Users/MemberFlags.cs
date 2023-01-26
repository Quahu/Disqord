namespace Disqord;

/// <summary>
///     Represents the flags of a member.
/// </summary>
public enum MemberFlags
{
    /// <summary>
    ///     The member has no flags.
    /// </summary>
    None = 0,

    /// <summary>
    ///     The member has left and rejoined the guild.
    /// </summary>
    Rejoined = 1 << 0,

    /// <summary>
    ///     The member has completed the onboarding process.
    /// </summary>
    CompletedOnboarding = 1 << 1,

    /// <summary>
    ///     The member bypasses the verification requirements of the guild.
    /// </summary>
    BypassesVerification = 1 << 2,

    /// <summary>
    ///     The member has started the onboarding process.
    /// </summary>
    StartedOnboarding = 1 << 3
}
