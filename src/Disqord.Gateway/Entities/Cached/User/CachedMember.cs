using System;
using System.Collections.Generic;
using System.ComponentModel;
using Qommon.Collections;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway
{
    public class CachedMember : CachedUser, IMember
    {
        public Snowflake GuildId { get; }

        public string Nick { get; private set; }

        public IReadOnlyList<Snowflake> RoleIds => _roleIds.ReadOnly();
        private Snowflake[] _roleIds;

        public Optional<DateTimeOffset> JoinedAt { get; private set; }

        public bool IsMuted { get; private set; }

        public bool IsDeafened { get; private set; }

        public DateTimeOffset? BoostedAt { get; private set; }

        public bool IsPending { get; private set; }

        public string GuildAvatarHash { get; private set; }

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

            Nick = model.Nick;
            _roleIds = model.Roles;

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
}
