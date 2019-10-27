using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed partial class RestMember : RestUser, IMember
    {
        public string Nick { get; private set; }

        public string DisplayName => Nick ?? Name;

        public IReadOnlyCollection<Snowflake> RoleIds { get; private set; }

        public DateTimeOffset JoinedAt { get; }

        public bool IsMuted { get; }

        public bool IsDeafened { get; }

        public Snowflake GuildId { get; }

        public RestDownloadable<RestGuild> Guild { get; }

        public override string Mention => Discord.MentionUser(this);

        public DateTimeOffset? BoostedAt { get; private set; }

        public bool IsBoosting => BoostedAt != null;

        internal RestMember(RestDiscordClient client, MemberModel model, Snowflake guildId) : base(client, model.User)
        {
            GuildId = guildId;
            Guild = new RestDownloadable<RestGuild>(options => Client.GetGuildAsync(GuildId, options));
            JoinedAt = model.JoinedAt;
            IsMuted = model.Mute;
            IsDeafened = model.Deaf;
            Update(model);
        }

        internal void Update(MemberModel model)
        {
            Nick = model.Nick.Value;
            var builder = ImmutableArray.CreateBuilder<Snowflake>();
            builder.Add(GuildId);
            for (var i = 0; i < model.Roles.Value.Length; i++)
                builder.Add(model.Roles.Value[i]);
            RoleIds = builder.ToImmutable();
            BoostedAt = model.PremiumSince.Value;

            base.Update(model.User);
        }
    }
}
