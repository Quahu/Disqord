using System;
using Disqord.Models;
using Qommon;

namespace Disqord.Gateway;

/// <inheritdoc cref="IThreadMetadata"/>
public class CachedThreadMetadata : CachedEntity, IThreadMetadata
{
    /// <inheritdoc/>
    public bool IsArchived { get; private set; }

    /// <inheritdoc/>
    public TimeSpan AutomaticArchiveDuration { get; private set; }

    /// <inheritdoc/>
    public DateTimeOffset ArchiveStateChangedAt { get; private set; }

    /// <inheritdoc/>
    public bool IsLocked { get; private set; }

    /// <inheritdoc/>
    public bool AllowsInvitation { get; private set; }

    /// <inheritdoc/>
    public DateTimeOffset? CreatedAt { get; }

    public CachedThreadMetadata(IGatewayClient client, ThreadMetadataJsonModel model)
        : base(client)
    {
        CreatedAt = model.CreateTimestamp.GetValueOrDefault();

        Update(model);
    }

    public void Update(ThreadMetadataJsonModel model)
    {
        IsArchived = model.Archived;
        AutomaticArchiveDuration = TimeSpan.FromMinutes(model.AutoArchiveDuration);
        ArchiveStateChangedAt = model.ArchiveTimestamp;
        IsLocked = model.Locked.GetValueOrDefault();
        AllowsInvitation = model.Invitable.GetValueOrDefault(true);
    }
}