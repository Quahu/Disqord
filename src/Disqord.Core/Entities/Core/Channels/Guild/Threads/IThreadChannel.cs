using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a thread channel.
    /// </summary>
    /// <remarks>
    ///     The <see cref="IGuildChannel.Position"/>, <see cref="IGuildChannel.Overwrites"/>, and <see cref="ICategorizableGuildChannel.CategoryId"/>
    ///     properties are not present for transient implementations of this type, as they are dependant on the parent channel being cached.
    /// </remarks>
    public interface IThreadChannel : IMessageGuildChannel, IChannelEntity, IMentionable, ITaggable
    {
        /// <summary>
        ///     Gets the ID of the creator of this thread.
        /// </summary>
        Snowflake CreatorId { get; }

        /// <summary>
        ///     Gets the current member of this thread.
        ///     Returns <see langword="null"/>, if the current member has not joined this thread.
        /// </summary>
        IThreadMember CurrentMember { get; }

        /// <summary>
        ///     Gets the approximate message count of this thread.
        /// </summary>
        /// <remarks>
        ///     This is not always a reliable property as Discord stops counting rather quickly.
        /// </remarks>
        int MessageCount { get; }

        /// <summary>
        ///     Gets the approximate member count of this thread.
        /// </summary>
        /// <remarks>
        ///     This is not always a reliable property as Discord stops counting rather quickly.
        /// </remarks>
        int MemberCount { get; }

        /// <summary>
        ///     Gets whether this thread is archived.
        /// </summary>
        bool IsArchived { get; }

        /// <summary>
        ///     Gets the automatic archive duration of this thread.
        /// </summary>
        TimeSpan AutomaticArchiveDuration { get; }

        /// <summary>
        ///     Gets the date of when this thread's archive state has last changed.
        ///     This also gets updated when the archive duration changes.
        /// </summary>
        DateTimeOffset ArchiveStateChangedAt { get; }

        /// <summary>
        ///     Gets whether this thread is locked, i.e. whether it was manually archived by a moderator.
        /// </summary>
        bool IsLocked { get; }
    }
}
