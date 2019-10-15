﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;
using Disqord.Rest.AuditLogs;
using Qommon.Collections;

namespace Disqord.Rest
{
    public sealed class RestGuild : RestSnowflakeEntity, IGuild
    {
        public string Name { get; private set; }

        public string IconHash { get; private set; }

        public string SplashHash { get; private set; }

        public Snowflake OwnerId { get; private set; }

        public string VoiceRegionId { get; private set; }

        public Snowflake? AfkChannelId { get; private set; }

        public int AfkTimeout { get; private set; }

        public bool IsEmbedEnabled { get; private set; }

        public Snowflake? EmbedChannelId { get; private set; }

        public VerificationLevel VerificationLevel { get; private set; }

        public DefaultNotificationLevel DefaultNotificationLevel { get; private set; }

        public ExplicitFilterLevel ExplicitFilterLevel { get; private set; }

        public IReadOnlyDictionary<Snowflake, RestRole> Roles { get; private set; }

        public IReadOnlyList<RestGuildEmoji> Emojis { get; private set; }

        public IReadOnlyList<string> Features { get; private set; }

        public MfaLevel MfaLevel { get; private set; }

        public Snowflake? ApplicationId { get; private set; }

        public bool IsWidgetEnabled { get; private set; }

        public Snowflake? WidgetChannelId { get; private set; }

        public Snowflake? SystemChannelId { get; private set; }

        public int MaxPresenceCount { get; private set; }

        public int MaxMemberCount { get; private set; }

        public string VanityUrlCode { get; private set; }

        public string Description { get; private set; }

        public string BannerHash { get; private set; }

        public BoostTier BoostTier { get; private set; }

        public int BoostingMemberCount { get; private set; }

        public CultureInfo PreferredLocale { get; private set; }

        IReadOnlyDictionary<Snowflake, IRole> IGuild.Roles => new ReadOnlyUpcastingDictionary<Snowflake, RestRole, IRole>(Roles);
        IReadOnlyList<IGuildEmoji> IGuild.Emojis => Emojis;

        internal RestGuild(RestDiscordClient client, GuildModel model) : base(client, model.Id)
        {
            Update(model);
        }

        internal void Update(GuildModel model)
        {
            if (model.Name.HasValue)
                Name = model.Name.Value;

            if (model.Icon.HasValue)
                IconHash = model.Icon.Value;

            if (model.Splash.HasValue)
                SplashHash = model.Splash.Value;

            if (model.OwnerId.HasValue)
                OwnerId = model.OwnerId.Value;

            if (model.Region.HasValue)
                VoiceRegionId = model.Region.Value;

            if (model.AfkChannelId.HasValue)
                AfkChannelId = model.AfkChannelId.Value;

            if (model.AfkTimeout.HasValue)
                AfkTimeout = model.AfkTimeout.Value;

            if (model.EmbedEnabled.HasValue)
                IsEmbedEnabled = model.EmbedEnabled.Value;

            if (model.EmbedChannelId.HasValue)
                EmbedChannelId = model.EmbedChannelId.Value;

            if (model.VerificationLevel.HasValue)
                VerificationLevel = model.VerificationLevel.Value;

            if (model.DefaultMessageNotifications.HasValue)
                DefaultNotificationLevel = model.DefaultMessageNotifications.Value;

            if (model.ExplicitContentFilterLevel.HasValue)
                ExplicitFilterLevel = model.ExplicitContentFilterLevel.Value;

            if (model.Roles.HasValue)
                Roles = new ReadOnlyDictionary<Snowflake, RestRole>(model.Roles.Value.ToDictionary(x => new Snowflake(x.Id), x =>
                {
                    var role = new RestRole(Client, x, Id);
                    role.Guild.SetValue(this);
                    return role;
                }));

            if (model.Emojis.HasValue)
                Emojis = model.Emojis.Value.Select(x =>
                {
                    var emoji = new RestGuildEmoji(Client, x, Id);
                    emoji.Guild.SetValue(this);
                    return emoji;
                }).ToImmutableArray();

            if (model.Features.HasValue)
                Features = model.Features.Value.ToImmutableArray();

            if (model.MfaLevel.HasValue)
                MfaLevel = model.MfaLevel.Value;

            if (model.ApplicationId.HasValue)
                ApplicationId = model.ApplicationId.Value;

            if (model.WidgetEnabled.HasValue)
                IsWidgetEnabled = model.WidgetEnabled.Value;

            if (model.WidgetChannelId.HasValue)
                WidgetChannelId = model.WidgetChannelId.Value;

            if (model.SystemChannelId.HasValue)
                SystemChannelId = model.SystemChannelId.Value;

            if (model.MaxPresences.HasValue)
                MaxPresenceCount = model.MaxPresences.Value ?? Discord.DEFAULT_MAX_PRESENCE_COUNT;

            if (model.MaxMembers.HasValue)
                MaxMemberCount = model.MaxMembers.Value;

            if (model.VanityUrlCode.HasValue)
                VanityUrlCode = model.VanityUrlCode.Value;

            if (model.Description.HasValue)
                Description = model.Description.Value;

            if (model.Banner.HasValue)
                BannerHash = model.Banner.Value;

            if (model.PremiumTier.HasValue)
                BoostTier = model.PremiumTier.Value;

            if (model.PremiumSubscriptionCount.HasValue && model.PremiumSubscriptionCount.Value != null)
                BoostingMemberCount = model.PremiumSubscriptionCount.Value.Value;

            if (model.PreferredLocale.HasValue)
                PreferredLocale = CultureInfo.ReadOnly(new CultureInfo(model.PreferredLocale.Value));
        }

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteGuildAsync(Id, options);

        public async Task<RestMember> GetMemberAsync(Snowflake id, RestRequestOptions options = null)
        {
            var member = await Client.GetMemberAsync(Id, id, options).ConfigureAwait(false);
            member.Guild.SetValue(this);
            return member;
        }

        public string GetIconUrl(ImageFormat? imageFormat = null, int size = 2048)
            => Discord.GetGuildIconUrl(Id, IconHash, imageFormat, size);

        public string GetSplashUrl(int size = 2048)
            => Discord.GetGuildSplashUrl(Id, SplashHash, ImageFormat.Png, 2048);

        public Task<IReadOnlyList<RestAuditLog>> GetAuditLogsAsync(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null)
            => GetAuditLogsAsync<RestAuditLog>(limit, userId, startFromId, options);

        public Task<IReadOnlyList<T>> GetAuditLogsAsync<T>(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog
            => Client.GetAuditLogsAsync<T>(Id, limit, userId, startFromId, options);

        public async Task<RestRole> CreateRoleAsync(Action<CreateRoleProperties> action = null, RestRequestOptions options = null)
        {
            var role = await Client.CreateRoleAsync(Id, action, options).ConfigureAwait(false);
            role.Guild.SetValue(this);
            return role;
        }

        public async Task<IReadOnlyList<RestBan>> GetBansAsync(RestRequestOptions options = null)
        {
            var bans = await Client.GetBansAsync(Id, options).ConfigureAwait(false);
            for (var i = 0; i < bans.Count; i++)
                bans[i].Guild.SetValue(this);

            return bans;
        }

        public override string ToString()
            => Name;
    }
}