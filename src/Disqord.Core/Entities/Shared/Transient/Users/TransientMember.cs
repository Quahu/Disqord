using System;
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public class TransientMember : TransientUser, IMember, ITransientEntity<MemberJsonModel>
    {
        public Snowflake GuildId { get; }

        public string Nick => Model.Nick;

        public IReadOnlyList<Snowflake> RoleIds => Model.Roles;

        public Optional<DateTimeOffset> JoinedAt => Model.JoinedAt;

        public bool IsMuted => Model.Mute.GetValueOrDefault();

        public bool IsDeafened => Model.Deaf.GetValueOrDefault();

        public DateTimeOffset? BoostedAt => Model.PremiumSince.GetValueOrDefault();

        public bool IsPending => Model.Pending.GetValueOrDefault();

        public new MemberJsonModel Model { get; }

        public TransientMember(IClient client, Snowflake guildId, MemberJsonModel model)
            : base(client, model.User.Value)
        {
            GuildId = guildId;
            Model = model;
        }
    }
}
