using System;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents the metadata of a thread channel.
/// </summary>
public interface IThreadMetadata : IEntity, IJsonUpdatable<ThreadMetadataJsonModel>
{
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
    /// </summary>
    /// <remarks>
    ///     This also gets updated when the archive duration changes.
    /// </remarks>
    DateTimeOffset ArchiveStateChangedAt { get; }

    /// <summary>
    ///     Gets whether this thread is locked, i.e. whether it was manually archived by a moderator.
    /// </summary>
    bool IsLocked { get; }

    /// <summary>
    ///     Gets whether non-moderators can add other non-moderators to this thread.
    /// </summary>
    bool AllowsInvitation { get; }

    /// <summary>
    ///     Gets the date of when this thread was created.
    /// </summary>
    /// <remarks>
    ///     This property is only available for threads created after January 9th 2022.
    /// </remarks>
    /// <returns>
    ///     The creation date of the thread or <see langword="null"/> for older threads (see remarks).
    /// </returns>
    DateTimeOffset? CreatedAt { get; }
}