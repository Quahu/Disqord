using System;
using System.Collections.Generic;
using System.ComponentModel;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedMember : CachedUser, IMember
    {
        public Snowflake GuildId { get; }

        public string Nick { get; private set; }

        public IReadOnlyList<Snowflake> RoleIds { get; private set; }

        public Optional<DateTimeOffset> JoinedAt { get; }

        public bool IsMuted { get; private set; }

        public bool IsDeafened { get; private set; }

        public DateTimeOffset? BoostedAt { get; private set; }

        public bool IsPending { get; }

        public CachedMember(ICachedSharedUser sharedUser, Snowflake guildId, MemberJsonModel model)
            : base(sharedUser)
        {
            GuildId = guildId;
            JoinedAt = model.JoinedAt;

            Update(model);
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Update(MemberJsonModel model)
        {
            if (model.User.HasValue)
                Update(model.User.Value);

            Nick = model.Nick;
            RoleIds = model.Roles.ReadOnly();
            IsMuted = model.Mute;
            IsDeafened = model.Deaf;

            if (model.PremiumSince.HasValue)
                BoostedAt = model.PremiumSince.Value;
        }
    }
}
