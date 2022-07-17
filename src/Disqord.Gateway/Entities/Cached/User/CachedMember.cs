using System;
using System.Collections.Generic;
using System.ComponentModel;
using Disqord.Models;
using Qommon;

namespace Disqord.Gateway;

public class CachedMember : CachedShareeUser, IMember
{
    /// <inheritdoc/>
    public Snowflake GuildId { get; }

    /// <inheritdoc/>
    public string? Nick { get; private set; }

    /// <inheritdoc/>
    public IReadOnlyList<Snowflake> RoleIds { get; private set; } = null!;

    /// <inheritdoc/>
    public Optional<DateTimeOffset> JoinedAt { get; private set; }

    /// <inheritdoc/>
    public bool IsMuted { get; private set; }

    /// <inheritdoc/>
    public bool IsDeafened { get; private set; }

    /// <inheritdoc/>
    public DateTimeOffset? BoostedAt { get; private set; }

    /// <inheritdoc/>
    public bool IsPending { get; private set; }

    /// <inheritdoc/>
    public string? GuildAvatarHash { get; private set; }

    /// <inheritdoc/>
    public DateTimeOffset? TimedOutUntil { get; private set; }

    public CachedMember(CachedSharedUser sharedUser, Snowflake guildId, MemberJsonModel model)
        : base(sharedUser)
    {
        GuildId = guildId;

        Update(model);
    }

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Update(MemberJsonModel model)
    {
        if (model.User.HasValue)
            Update(model.User.Value);

        Nick = model.Nick.GetValueOrDefault();
        RoleIds = model.Roles;

        if (model.JoinedAt.HasValue)
            JoinedAt = model.JoinedAt.Value;

        if (model.Mute.HasValue)
            IsMuted = model.Mute.Value;

        if (model.Deaf.HasValue)
            IsDeafened = model.Deaf.Value;

        if (model.PremiumSince.HasValue)
            BoostedAt = model.PremiumSince.Value;

        IsPending = model.Pending.GetValueOrDefault();

        if (model.Avatar.HasValue)
            GuildAvatarHash = model.Avatar.Value;

        if (model.CommunicationDisabledUntil.HasValue)
            TimedOutUntil = model.CommunicationDisabledUntil.Value;
    }
}
