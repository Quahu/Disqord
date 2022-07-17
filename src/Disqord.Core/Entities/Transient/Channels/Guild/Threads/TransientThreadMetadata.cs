using System;
using Disqord.Models;
using Qommon;

namespace Disqord;

/// <inheritdoc cref="IThreadMetadata"/>
public class TransientThreadMetadata : TransientEntity<ThreadMetadataJsonModel>, IThreadMetadata
{
    /// <inheritdoc/>
    public bool IsArchived => Model.Archived;

    /// <inheritdoc/>
    public TimeSpan AutomaticArchiveDuration => TimeSpan.FromMinutes(Model.AutoArchiveDuration);

    /// <inheritdoc/>
    public DateTimeOffset ArchiveStateChangedAt => Model.ArchiveTimestamp;

    /// <inheritdoc/>
    public bool IsLocked => Model.Locked.GetValueOrDefault();

    /// <inheritdoc/>
    public bool AllowsInvitation => Model.Invitable.GetValueOrDefault(true);

    /// <inheritdoc/>
    public DateTimeOffset? CreatedAt => Model.CreateTimestamp.GetValueOrDefault();

    public TransientThreadMetadata(ThreadMetadataJsonModel model)
        : base(model)
    { }
}