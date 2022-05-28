namespace Disqord.Bot.Commands;

/// <summary>
///     Represents the type of the rate-limit bucket.
/// </summary>
public enum RateLimitBucketType
{
    /// <summary>
    ///     The rate-limit bucket applies per-user.
    /// </summary>
    User = 0,

    /// <summary>
    ///     The rate-limit bucket applies per-member,
    ///     i.e. not globally, but within the given guild.
    /// </summary>
    Member = 1,

    /// <summary>
    ///     The rate-limit bucket applies per-guild,
    /// </summary>
    Guild = 2,

    /// <summary>
    ///     The rate-limit bucket applies per-channel,
    /// </summary>
    Channel = 3
}
