using System;

namespace Disqord.Gateway;

/// <summary>
///     Represents a member's rich activity.
/// </summary>
public interface IRichActivity : IActivity
{
    /// <summary>
    ///     Gets the state of this activity.
    /// </summary>
    string? State { get; }

    /// <summary>
    ///     Gets the details of this activity.
    /// </summary>
    string? Details { get; }

    /// <summary>
    ///     Gets when this activity started.
    /// </summary>
    /// <returns>
    ///     When the activity started at or <see langword="null"/> if there is no start timestamp or its value is invalid.
    /// </returns>
    DateTimeOffset? StartedAt { get; }

    /// <summary>
    ///     Gets when this activity ends.
    /// </summary>
    /// <returns>
    ///     When the activity ends at or <see langword="null"/> if there is no end timestamp or its value is invalid.
    /// </returns>
    DateTimeOffset? EndsAt { get; }

    /// <summary>
    ///     Gets the large asset of this activity.
    /// </summary>
    IRichActivityAsset? LargeAsset { get; }

    /// <summary>
    ///     Gets the small asset of this activity.
    /// </summary>
    IRichActivityAsset? SmallAsset { get; }

    /// <summary>
    ///     Gets the party of this activity.
    /// </summary>
    IRichActivityParty? Party { get; }

    /// <summary>
    ///     Gets the match secret of this activity.
    /// </summary>
    string? MatchSecret { get; }

    /// <summary>
    ///     Gets the join secret of this activity.
    /// </summary>
    string? JoinSecret { get; }

    /// <summary>
    ///     Gets the spectate secret of this activity.
    /// </summary>
    string? SpectateSecret { get; }

    /// <summary>
    ///     Gets whether this activity is an instanced game session.
    /// </summary>
    bool IsInstanced { get; }

    /// <summary>
    ///     Gets the ID of the application of this activity.
    /// </summary>
    Snowflake? ApplicationId { get; }

    /// <summary>
    ///     Gets the sync ID of this activity.
    /// </summary>
    string? SyncId { get; }

    /// <summary>
    ///     Gets the ID of the session of this activity.
    /// </summary>
    string? SessionId { get; }

    /// <summary>
    ///     Gets the flags of this activity.
    /// </summary>
    ActivityFlags? Flags { get; }
}
