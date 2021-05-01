using System;

namespace Disqord.Gateway
{
    /// <summary>
    ///     Represents a member's rich activity.
    /// </summary>
    public interface IRichActivity : IActivity
    {
        /// <summary>
        ///     Gets the state of this activity.
        ///     Returns <see langword="null"/> if there is none.
        /// </summary>
        string State { get; }

        /// <summary>
        ///     Gets the details of this activity.
        ///     Returns <see langword="null"/> if there is none.
        /// </summary>
        string Details { get; }

        /// <summary>
        ///     Gets when this activity started.
        ///     Returns <see langword="null"/> if there is start timestamp.
        /// </summary>
        DateTimeOffset? StartedAt { get; }

        /// <summary>
        ///     Gets when this activity ends.
        ///     Returns <see langword="null"/> if there is end timestamp.
        /// </summary>
        DateTimeOffset? EndsAt { get; }

        /// <summary>
        ///     Gets the large asset of this activity.
        ///     Returns <see langword="null"/> if there is none.
        /// </summary>
        RichActivityAsset LargeAsset { get; }

        /// <summary>
        ///     Gets the small asset of this activity.
        ///     Returns <see langword="null"/> if there is none.
        /// </summary>
        RichActivityAsset SmallAsset { get; }

        /// <summary>
        ///     Gets the party of this activity.
        ///     Returns <see langword="null"/> if there is none.
        /// </summary>
        RichActivityParty Party { get; }

        /// <summary>
        ///     Gets the match secret of this activity.
        ///     Returns <see langword="null"/> if there is none.
        /// </summary>
        string MatchSecret { get; }

        /// <summary>
        ///     Gets the join secret of this activity.
        ///     Returns <see langword="null"/> if there is none.
        /// </summary>
        string JoinSecret { get; }

        /// <summary>
        ///     Gets the spectate secret of this activity.
        ///     Returns <see langword="null"/> if there is none.
        /// </summary>
        string SpectateSecret { get; }

        /// <summary>
        ///     Gets whether this activity is an instanced game session.
        /// </summary>
        bool IsInstanced { get; }

        /// <summary>
        ///     Gets the application ID of this entity.
        ///     Returns <see langword="null"/> if there is none.
        /// </summary>
        Snowflake? ApplicationId { get; }

        /// <summary>
        ///     Gets the sync ID of this activity.
        ///     Returns <see langword="null"/> if there is none.
        /// </summary>
        string SyncId { get; }

        /// <summary>
        ///     Gets the session ID of this activity.
        ///     Returns <see langword="null"/> if there is none.
        /// </summary>
        string SessionId { get; }

        /// <summary>
        ///     Gets the flags of this activity.
        ///     Returns <see langword="null"/> if there is none.
        /// </summary>
        ActivityFlags? Flags { get; }
    }
}
