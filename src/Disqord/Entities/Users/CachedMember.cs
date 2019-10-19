using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Models.Dispatches;
using Qommon.Collections;

namespace Disqord
{
    public sealed partial class CachedMember : CachedUser, IMember
    {
        public override string Name => SharedUser.Name;

        public override string Discriminator => SharedUser.Discriminator;

        public override string AvatarHash => SharedUser.AvatarHash;

        public override bool IsBot => SharedUser.IsBot;

        public string Nick { get; private set; }

        public IReadOnlyDictionary<Snowflake, CachedRole> Roles { get; }

        public DateTimeOffset JoinedAt { get; }

        public GuildPermissions Permissions => Discord.Permissions.CalculatePermissions(Guild, this, Roles.Values);

        public bool IsMuted { get; private set; }

        public bool IsDeafened { get; private set; }

        public Snowflake? VoiceChannelId { get; private set; }

        public CachedVoiceChannel VoiceChannel
        {
            get
            {
                var voiceChannelId = VoiceChannelId;
                return voiceChannelId != null
                    ? Guild.GetVoiceChannel(voiceChannelId.Value)
                    : null;
            }
        }

        public string VoiceSessionId { get; private set; }

        public bool IsSelfDeafened { get; private set; }

        public bool IsSelfMuted { get; private set; }

        public bool IsSuppressed { get; private set; }

        public override UserStatus Status => _status ?? SharedUser.Status;
        private UserStatus? _status;

        public override Activity Activity => _activity ?? SharedUser.Activity;
        private Activity _activity;

        public CachedGuild Guild { get; }

        public override string Mention => Discord.MentionUser(this);

        public int Hierarchy => Id == Guild.OwnerId
            ? int.MaxValue
            : Roles.Values.Max(x => x.Position);

        public Color? Color
        {
            get
            {
                if (Roles.Count == 1)
                    return null;

                var role = Roles.Values.OrderByDescending(x => x.Position).FirstOrDefault(x => x.Color != default);
                return role?.Color;
            }
        }

        public string DisplayName => Nick ?? Name;

        public DateTimeOffset? BoostedAt { get; private set; }

        public bool IsBoosting => BoostedAt != null;

        private readonly ConcurrentDictionary<Snowflake, CachedRole> _roles;

        internal override CachedSharedUser SharedUser { get; }

        Snowflake IMember.GuildId => Guild.Id;
        IReadOnlyCollection<Snowflake> IMember.RoleIds => new ReadOnlyCollection<Snowflake>(_roles.Keys);

        internal CachedMember(DiscordClient client, MemberModel model, CachedGuild guild, CachedSharedUser user) : base(client, user.Id)
        {
            _roles = Extensions.CreateConcurrentDictionary<Snowflake, CachedRole>(model.Roles.Value.Length);
            Roles = new ReadOnlyDictionary<Snowflake, CachedRole>(_roles);
            SharedUser = user;
            Guild = guild;
            JoinedAt = model.JoinedAt;
            IsMuted = model.Mute;
            IsDeafened = model.Deaf;

            Update(model);
        }

        internal override void Update(UserModel model)
            => SharedUser.Update(model);

        internal void Update(MemberModel model)
        {
            if (model.Nick.HasValue)
                Nick = model.Nick.Value;

            if (model.Roles.HasValue)
            {
                _roles.Clear();
                _roles[Guild.Id] = Guild.Roles[Guild.Id];
                for (var i = 0; i < model.Roles.Value.Length; i++)
                {
                    var roleId = model.Roles.Value[i];
                    if (!Guild.Roles.TryGetValue(roleId, out var role))
                    {
                        Client.Log(LogMessageSeverity.Error, $"Guild has no role the member has. Id: {roleId} {Guild.Name}.");
                        continue;
                    }
                    _roles[roleId] = role;
                }
            }

            if (model.PremiumSince.HasValue)
                BoostedAt = model.PremiumSince.Value;

            if (model.User != null)
                Update(model.User);
        }

        internal void Update(GuildMemberUpdateModel model)
        {
            if (model.Nick.HasValue)
                Nick = model.Nick.Value;

            if (model.Roles.HasValue)
            {
                _roles.Clear();
                _roles[Guild.Id] = Guild.Roles[Guild.Id];
                for (var i = 0; i < model.Roles.Value.Length; i++)
                {
                    var roleId = model.Roles.Value[i];
                    if (!Guild.Roles.TryGetValue(roleId, out var role))
                    {
                        Client.Log(LogMessageSeverity.Error, $"Guild ({Guild.Name}) has no role the member has. Id: {roleId}.");
                        continue;
                    }
                    _roles[roleId] = role;
                }
            }
        }

        internal void Update(VoiceStateModel model)
        {
            if (model.ChannelId.HasValue)
            {
                VoiceChannelId = model.ChannelId.Value;

                if (model.ChannelId.Value == null)
                {
                    VoiceSessionId = null;
                    IsDeafened = false;
                    IsMuted = false;
                    IsSelfDeafened = false;
                    IsSelfMuted = false;
                    IsSuppressed = false;
                    return;
                }
            }

            if (model.SessionId.HasValue)
                VoiceSessionId = model.SessionId.Value;

            if (model.Deaf.HasValue)
                IsDeafened = model.Deaf.Value;

            if (model.Mute.HasValue)
                IsMuted = model.Mute.Value;

            if (model.SelfDeaf.HasValue)
                IsSelfDeafened = model.SelfDeaf.Value;

            if (model.SelfMute.HasValue)
                IsSelfMuted = model.SelfMute.Value;

            if (model.Suppress.HasValue)
                IsSuppressed = model.Suppress.Value;
        }

        internal override void Update(PresenceUpdateModel model)
        {
            _status = model.Status;
            _activity = model.Activity != null
                ? Activity.Create(model.Activity)
                : null;

            base.Update(model);
        }

        internal new CachedMember Clone()
            => (CachedMember) MemberwiseClone();

        public ChannelPermissions GetPermissionsFor(IGuildChannel channel)
            => Discord.Permissions.CalculatePermissions(Guild, channel, this, Roles.Values);
    }
}
