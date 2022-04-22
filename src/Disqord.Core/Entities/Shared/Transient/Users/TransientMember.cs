using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientMember : TransientUser, IMember, ITransientClientEntity<MemberJsonModel>
    {
        /// <inheritdoc/>
        public Snowflake GuildId { get; }

        /// <inheritdoc/>
        public string Nick => Model.Nick;

        /// <inheritdoc/>
        public IReadOnlyList<Snowflake> RoleIds => Model.Roles;

        /// <inheritdoc/>
        public Optional<DateTimeOffset> JoinedAt => Model.JoinedAt;

        /// <inheritdoc/>
        public bool IsMuted => Model.Mute.GetValueOrDefault();

        /// <inheritdoc/>
        public bool IsDeafened => Model.Deaf.GetValueOrDefault();

        /// <inheritdoc/>
        public DateTimeOffset? BoostedAt => Model.PremiumSince.GetValueOrDefault();

        /// <inheritdoc/>
        public bool IsPending => Model.Pending.GetValueOrDefault();

        /// <inheritdoc/>
        public string GuildAvatarHash => Model.Avatar.GetValueOrDefault();

        /// <inheritdoc/>
        public DateTimeOffset? TimedOutUntil => Model.CommunicationDisabledUntil.GetValueOrDefault();

        /// <inheritdoc/>
        public new MemberJsonModel Model { get; }

        public TransientMember(IClient client, Snowflake guildId, MemberJsonModel model)
            : base(client, model.User.Value)
        {
            GuildId = guildId;
            Model = model;
        }
    }
}
