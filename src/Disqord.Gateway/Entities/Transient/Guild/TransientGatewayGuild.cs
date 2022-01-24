using System;
using System.Collections.Generic;
using System.Globalization;
using Disqord.Gateway;
using Disqord.Gateway.Api.Models;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    public class TransientGatewayGuild : TransientGatewayClientEntity<GatewayGuildJsonModel>, IGatewayGuild, ITransientEntity<GuildJsonModel>
    {
        public Snowflake Id => Model.Id;

        public string Name => Model.Name;

        public string IconHash => Model.Icon;

        public string SplashHash => Model.Splash;

        public string DiscoverySplashHash => Model.DiscoverySplash.GetValueOrDefault();

        public Snowflake OwnerId => Model.OwnerId;

        public Snowflake? AfkChannelId => Model.AfkChannelId;

        public TimeSpan AfkTimeout => TimeSpan.FromSeconds(Model.AfkTimeout);

        public bool IsWidgetEnabled => Model.WidgetEnabled.GetValueOrDefault();

        public Snowflake? WidgetChannelId => Model.WidgetChannelId.GetValueOrDefault();

        public GuildVerificationLevel VerificationLevel => Model.VerificationLevel;

        public GuildNotificationLevel NotificationLevel => Model.DefaultMessageNotifications;

        public GuildContentFilterLevel ContentFilterLevel => Model.ExplicitContentFilter;

        public IReadOnlyDictionary<Snowflake, IRole> Roles => _roles ??= Model.Roles.ToReadOnlyDictionary((Client, Id),
            (model, _) => model.Id,
            (model, state) =>
            {
                var (client, guildId) = state;
                return new TransientRole(client, guildId, model) as IRole;
            });
        private IReadOnlyDictionary<Snowflake, IRole> _roles;

        public IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis => _emojis ??= Model.Emojis.ToReadOnlyDictionary((Client, Id),
            (model, _) => model.Id.Value,
            (model, state) =>
            {
                var (client, guildId) = state;
                return new TransientGuildEmoji(client, guildId, model) as IGuildEmoji;
            });
        private IReadOnlyDictionary<Snowflake, IGuildEmoji> _emojis;

        public IReadOnlyList<string> Features => Model.Features;

        public GuildMfaLevel MfaLevel => Model.MfaLevel;

        public Snowflake? ApplicationId => Model.ApplicationId;

        public Snowflake? SystemChannelId => Model.SystemChannelId;

        public SystemChannelFlag SystemChannelFlags => Model.SystemChannelFlags;

        public Snowflake? RulesChannelId => Model.RulesChannelId;

        public int? MaxPresenceCount => Model.MaxPresences.GetValueOrDefault();

        public int? MaxMemberCount => Model.MaxMembers.GetValueOrNullable();

        public string VanityUrlCode => Model.VanityUrlCode;

        public string Description => Model.Description;

        public string BannerHash => Model.Banner;

        public GuildBoostTier BoostTier => Model.PremiumTier;

        public int? BoostingMemberCount => Model.PremiumSubscriptionCount.GetValueOrNullable();

        public CultureInfo PreferredLocale => Discord.Internal.GetLocale(Model.PreferredLocale);

        public Snowflake? PublicUpdatesChannelId => Model.PublicUpdatesChannelId;

        public int? MaxVideoMemberCount => Model.MaxVideoChannelUsers.GetValueOrNullable();

        public GuildNsfwLevel NsfwLevel => Model.NsfwLevel;

        public IReadOnlyDictionary<Snowflake, IGuildSticker> Stickers
        {
            get
            {
                if (!Model.Stickers.HasValue)
                    return ReadOnlyDictionary<Snowflake, IGuildSticker>.Empty;

                return _stickers ??= Model.Stickers.Value.ToReadOnlyDictionary(Client,
                    (model, _) => model.Id,
                    (model, client) => new TransientGuildSticker(client, model) as IGuildSticker);
            }
        }
        private IReadOnlyDictionary<Snowflake, IGuildSticker> _stickers;

        public bool IsBoostProgressBarEnabled => Model.PremiumProgressBarEnabled;

        public DateTimeOffset JoinedAt => Model.JoinedAt;

        public bool IsLarge => Model.Large;

        public bool IsUnavailable => Model.Unavailable.GetValueOrDefault();

        public int MemberCount => Model.MemberCount;

        public IReadOnlyDictionary<Snowflake, IVoiceState> VoiceStates => _voiceStates ??= Model.VoiceStates.ToReadOnlyDictionary(Client,
            (model, _) => model.UserId,
            (model, client) => new TransientVoiceState(client, model) as IVoiceState);
        private IReadOnlyDictionary<Snowflake, IVoiceState> _voiceStates;

        public IReadOnlyDictionary<Snowflake, IMember> Members => _members ??= Model.Members.ToReadOnlyDictionary((Client, Id),
            (model, _) => model.User.Value.Id, (model, state) =>
            {
                var (client, guildId) = state;
                return new TransientMember(client, guildId, model) as IMember;
            });
        private IReadOnlyDictionary<Snowflake, IMember> _members;

        public IReadOnlyDictionary<Snowflake, IGuildChannel> Channels => _channels ??= Model.Channels.ToReadOnlyDictionary(Client,
            (model, _) => model.Id,
            (model, client) => TransientGuildChannel.Create(client, model) as IGuildChannel);
        private IReadOnlyDictionary<Snowflake, IGuildChannel> _channels;

        public IReadOnlyDictionary<Snowflake, IPresence> Presences => _presences ??= Model.Presences.ToReadOnlyDictionary(Client,
            (model, _) => model.User.Id,
            (model, client) => new TransientPresence(client, model) as IPresence);
        private IReadOnlyDictionary<Snowflake, IPresence> _presences;

        public IReadOnlyDictionary<Snowflake, IStage> Stages => _stages ??= Model.StageInstances.ToReadOnlyDictionary(Client,
            (model, _) => model.Id,
            (model, client) => new TransientStage(client, model) as IStage);
        private IReadOnlyDictionary<Snowflake, IStage> _stages;

        public IReadOnlyDictionary<Snowflake, IGuildEvent> GuildEvents => _guildEvents ??= Model.GuildScheduledEvents.ToReadOnlyDictionary(Client,
            (model, _) => model.Id,
            (model, client) => new TransientGuildEvent(client, model) as IGuildEvent);
        private IReadOnlyDictionary<Snowflake, IGuildEvent> _guildEvents;

        GuildJsonModel ITransientEntity<GuildJsonModel>.Model => Model;

        public TransientGatewayGuild(IClient client, GatewayGuildJsonModel model)
            : base(client, model)
        { }

        public override string ToString()
            => this.GetString();
    }
}
