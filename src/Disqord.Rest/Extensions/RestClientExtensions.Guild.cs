﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Http;
using Disqord.Models;
using Disqord.Rest.Api;
using Disqord.Rest.Pagination;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        // TODO: create guild?

        public static async Task<IGuild> FetchGuildAsync(this IRestClient client, Snowflake guildId, bool? withCounts = null, IRestRequestOptions options = null)
        {
            try
            {
                var model = await client.ApiClient.FetchGuildAsync(guildId, withCounts, options).ConfigureAwait(false);
                return new TransientGuild(client, model);
            }
            catch (RestApiException ex) when (ex.StatusCode == HttpResponseStatusCode.NotFound)
            {
                return null;
            }
        }

        public static async Task<IGuild> ModifyGuildAsync(this IRestClient client, Snowflake guildId, Action<ModifyGuildActionProperties> action, IRestRequestOptions options = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyGuildActionProperties();
            action(properties);
            var content = new ModifyGuildJsonRestRequestContent
            {
                Name = properties.Name,
                Region = properties.VoiceRegionId,
                VerificationLevel = properties.VerificationLevel,
                DefaultMessageNotifications = properties.NotificationLevel,
                ExplicitContentFilter = properties.ContentFilterLevel,
                AfkChannelId = properties.AfkChannelId,
                AfkTimeout = properties.AfkTimeout,
                Icon = properties.Icon,
                OwnerId = properties.OwnerId,
                Splash = properties.Splash,
                Banner = properties.Banner,
                SystemChannelId = properties.SystemChannelId,
                RulesChannelId = properties.RulesChannelId,
                PublicUpdatesChannelId = properties.PublicUpdatesChannelId,
                PreferredLocale = Optional.Convert(properties.PreferredLocale, x => x.Name)
            };
            var model = await client.ApiClient.ModifyGuildAsync(guildId, content, options).ConfigureAwait(false);
            return new TransientGuild(client, model);
        }

        public static Task DeleteGuildAsync(this IRestClient client, Snowflake guildId, IRestRequestOptions options = null)
            => client.ApiClient.DeleteGuildAsync(guildId, options);

        public static async Task<IReadOnlyList<IGuildChannel>> FetchChannelsAsync(this IRestClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchGuildChannelsAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => TransientGuildChannel.Create(client, x));
        }

        public static async Task<ITextChannel> CreateTextChannelAsync(this IRestClient client, Snowflake guildId, string name, Action<CreateTextChannelActionProperties> action = null, IRestRequestOptions options = null)
        {
            var model = await client.InternalCreateGuildChannelAsync(guildId, name, action, options).ConfigureAwait(false);
            return new TransientTextChannel(client, model);
        }

        public static async Task<IVoiceChannel> CreateVoiceChannelAsync(this IRestClient client, Snowflake guildId, string name, Action<CreateVoiceChannelActionProperties> action = null, IRestRequestOptions options = null)
        {
            var model = await client.InternalCreateGuildChannelAsync(guildId, name, action, options).ConfigureAwait(false);
            return new TransientVoiceChannel(client, model);
        }

        public static async Task<ICategoryChannel> CreateCategoryChannelAsync(this IRestClient client, Snowflake guildId, string name, Action<CreateCategoryChannelActionProperties> action = null, IRestRequestOptions options = null)
        {
            var model = await client.InternalCreateGuildChannelAsync(guildId, name, action, options).ConfigureAwait(false);
            return new TransientCategoryChannel(client, model);
        }

        internal static Task<ChannelJsonModel> InternalCreateGuildChannelAsync<T>(this IRestClient client, Snowflake guildId, string name, Action<T> action, IRestRequestOptions options = null)
            where T : CreateGuildChannelActionProperties
        {
            // Can't use the new() generic constraint because the constructors are internal.
            // Can't use the generic CreateInstance either *because*.
            var properties = (T) Activator.CreateInstance(typeof(T), true);
            action?.Invoke(properties);
            var content = new CreateGuildChannelJsonRestRequestContent(name)
            {
                PermissionOverwrites = Optional.Convert(properties.Overwrites, x => x.Select(x => new OverwriteJsonModel
                {
                    Id = x.TargetId,
                    Type = x.TargetType,
                    Allow = x.Permissions.Allowed,
                    Deny = x.Permissions.Denied
                }).ToArray())
            };

            if (properties is CreateNestedChannelActionProperties nestedProperties)
            {
                content.ParentId = nestedProperties.ParentId;

                if (properties is CreateTextChannelActionProperties textProperties)
                {
                    if (textProperties.Topic.HasValue && textProperties.Topic.Value != null && textProperties.Topic.Value.Length > 1024)
                        throw new ArgumentOutOfRangeException(nameof(CreateTextChannelActionProperties.Topic));

                    content.Type = ChannelType.Text;
                    content.Topic = textProperties.Topic;
                    content.RateLimitPerUser = textProperties.Slowmode;
                    content.Nsfw = textProperties.IsNsfw;
                }
                else if (properties is CreateVoiceChannelActionProperties voiceProperties)
                {
                    content.Type = ChannelType.Voice;
                    content.Bitrate = voiceProperties.Bitrate;
                    content.UserLimit = voiceProperties.UserLimit;
                }
                else
                {
                    throw new ArgumentException($"Unknown channel action properties provided ({properties.GetType()}).");
                }
            }
            else if (properties is CreateCategoryChannelActionProperties categoryProperties)
            {
                content.Type = ChannelType.Category;
                // No extra properties for category channels.
            }
            else
            {
                throw new ArgumentException($"Unknown channel action properties provided ({properties.GetType()}).");
            }

            return client.ApiClient.CreateGuildChannelAsync(guildId, content, options);
        }

        public static Task ReorderChannelsAsync(this IRestClient client, Snowflake guildId, IReadOnlyDictionary<Snowflake, int> positions, IRestRequestOptions options = null)
        {
            if (positions == null)
                throw new ArgumentNullException(nameof(positions));

            var content = new JsonObjectRestRequestContent<ReorderJsonRestRequestContent[]>(positions.Select(x => new ReorderJsonRestRequestContent
            {
                Id = x.Key,
                Position = x.Value
            }).ToArray());
            return client.ApiClient.ReorderGuildChannelsAsync(guildId, content, options);
        }

        public static async Task<IMember> FetchMemberAsync(this IRestClient client, Snowflake guildId, Snowflake memberId, IRestRequestOptions options = null)
        {
            try
            {
                var model = await client.ApiClient.FetchMemberAsync(guildId, memberId, options).ConfigureAwait(false);
                return new TransientMember(client, guildId, model);
            }
            catch (RestApiException ex) when (ex.ErrorModel.Code == RestApiErrorCode.UnknownMember)
            {
                return null;
            }
        }

        public static IPagedEnumerable<IMember> EnumerateMembers(this IRestClient client, Snowflake guildId, int limit, Snowflake? startFromId = null, IRestRequestOptions options = null)
            => new PagedEnumerable<IMember>(new FetchMembersPagedEnumerator(client, guildId, limit, startFromId, options));

        public static Task<IReadOnlyList<IMember>> FetchMembersAsync(this IRestClient client, Snowflake guildId, int limit = 1000, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            if (limit == 0)
                return Task.FromResult(ReadOnlyList<IMember>.Empty);

            if (limit <= 1000)
                return client.InternalFetchMembersAsync(guildId, limit, startFromId, options);

            var enumerator = client.EnumerateMembers(guildId, limit, startFromId, options);
            return enumerator.FlattenAsync();
        }

        internal static async Task<IReadOnlyList<IMember>> InternalFetchMembersAsync(this IRestClient client, Snowflake guildId, int limit, Snowflake? startFromId, IRestRequestOptions options)
        {
            var models = await client.ApiClient.FetchMembersAsync(guildId, limit, startFromId, options).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), (x, tuple) =>
            {
                var (client, guildId) = tuple;
                return new TransientMember(client, guildId, x);
            });
        }

        public static Task SetCurrentMemberNickAsync(this IRestClient client, Snowflake guildId, string nick, IRestRequestOptions options = null)
        {
            var content = new SetOwnNickJsonRestRequestContent
            {
                Nick = nick
            };
            return client.ApiClient.SetOwnNickAsync(guildId, content, options);
        }

        public static async Task ModifyMemberAsync(this IRestClient client, Snowflake guildId, Snowflake memberId, Action<ModifyMemberActionProperties> action, IRestRequestOptions options = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyMemberActionProperties();
            action(properties);
            if (properties.Nick.HasValue && client.ApiClient.Token is BotToken botToken)
            {
                if (memberId == botToken.Id)
                {
                    await client.SetCurrentMemberNickAsync(guildId, properties.Nick.Value, options).ConfigureAwait(false);
                    properties.Nick = Optional<string>.Empty;
                }
            }

            var content = new ModifyMemberJsonRestRequestContent
            {
                Nick = properties.Nick,
                Roles = Optional.Convert(properties.RoleIds, x => x.ToArray()),
                ChannelId = properties.VoiceChannelId,
                Mute = properties.Mute,
                Deaf = properties.Deaf
            };
            await client.ApiClient.ModifyMemberAsync(guildId, memberId, content, options).ConfigureAwait(false);
        }

        public static Task GrantRoleAsync(this IRestClient client, Snowflake guildId, Snowflake memberId, Snowflake roleId, IRestRequestOptions options = null)
            => client.ApiClient.GrantRoleAsync(guildId, memberId, roleId, options);

        public static Task RevokeRoleAsync(this IRestClient client, Snowflake guildId, Snowflake memberId, Snowflake roleId, IRestRequestOptions options = null)
            => client.ApiClient.RevokeRoleAsync(guildId, memberId, roleId, options);

        public static Task KickMemberAsync(this IRestClient client, Snowflake guildId, Snowflake memberId, IRestRequestOptions options = null)
            => client.ApiClient.KickMemberAsync(guildId, memberId, options);

        public static async Task<IReadOnlyList<IBan>> FetchBansAsync(this IRestClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchBansAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), (x, tuple) =>
            {
                var (client, guildId) = tuple;
                return new TransientBan(client, guildId, x);
            });
        }

        public static async Task<IBan> FetchBanAsync(this IRestClient client, Snowflake guildId, Snowflake userId, IRestRequestOptions options = null)
        {
            try
            {
                var model = await client.ApiClient.FetchBanAsync(guildId, userId, options).ConfigureAwait(false);
                return new TransientBan(client, guildId, model);
            }
            catch (RestApiException ex) when (ex.ErrorModel.Code == RestApiErrorCode.UnknownBan)
            {
                return null;
            }
        }

        //public Task BanMemberAsync(Snowflake guildId, Snowflake memberId, string reason = null, int? deleteMessageDays = null, IRestRequestOptions options = null)
        //    => ApiClient.CreateGuildBanAsync(guildId, memberId, reason, deleteMessageDays, options);

        //public Task UnbanMemberAsync(Snowflake guildId, Snowflake userId, IRestRequestOptions options = null)
        //    => ApiClient.RemoveGuildBanAsync(guildId, userId, options);

        //public async Task<IReadOnlyList<RestRole>> GetRolesAsync(Snowflake guildId, IRestRequestOptions options = null)
        //{
        //    var models = await ApiClient.GetGuildRolesAsync(guildId, options).ConfigureAwait(false);
        //    return models.ToReadOnlyList((this, guildId), (x, tuple) =>
        //    {
        //        var (@this, guildId) = tuple;
        //        return new RestRole(@this, guildId, x);
        //    });
        //}

        //public async Task<RestRole> CreateRoleAsync(Snowflake guildId, Action<CreateRoleProperties> action = null, IRestRequestOptions options = null)
        //{
        //    var properties = new CreateRoleProperties();
        //    action?.Invoke(properties);
        //    var model = await ApiClient.CreateGuildRoleAsync(guildId, properties, options).ConfigureAwait(false);
        //    return new RestRole(this, guildId, model);
        //}

        //public async Task<IReadOnlyList<RestRole>> ReorderRolesAsync(Snowflake guildId, IReadOnlyDictionary<Snowflake, int> positions, IRestRequestOptions options = null)
        //{
        //    if (positions == null)
        //        throw new ArgumentNullException(nameof(positions));

        //    var models = await ApiClient.ModifyGuildRolePositionsAsync(guildId, positions, options).ConfigureAwait(false);
        //    return models.ToReadOnlyList((this, guildId), (x, tuple) =>
        //    {
        //        var (@this, guildId) = tuple;
        //        return new RestRole(@this, guildId, x);
        //    });
        //}

        //public async Task<RestRole> ModifyRoleAsync(Snowflake guildId, Snowflake roleId, Action<ModifyRoleProperties> action, IRestRequestOptions options = null)
        //{
        //    var model = await InternalModifyRoleAsync(guildId, roleId, action, options).ConfigureAwait(false);
        //    return new RestRole(this, guildId, model);
        //}

        //internal async Task<RoleModel> InternalModifyRoleAsync(Snowflake guildId, Snowflake roleId, Action<ModifyRoleProperties> action, IRestRequestOptions options = null)
        //{
        //    if (action == null)
        //        throw new ArgumentNullException(nameof(action));

        //    var properties = new ModifyRoleProperties();
        //    action(properties);
        //    if (properties.Position.HasValue)
        //    {
        //        await ReorderRolesAsync(guildId, new Dictionary<Snowflake, int>
        //        {
        //            [roleId] = properties.Position.Value
        //        }, options).ConfigureAwait(false);
        //    }

        //    return await ApiClient.ModifyGuildRoleAsync(guildId, roleId, properties, options).ConfigureAwait(false);
        //}

        //public Task DeleteRoleAsync(Snowflake guildId, Snowflake roleId, IRestRequestOptions options = null)
        //    => ApiClient.DeleteGuildRoleAsync(guildId, roleId, options);

        //public Task<int> GetPruneCountAsync(Snowflake guildId, int days, IRestRequestOptions options = null)
        //    => ApiClient.GetGuildPruneCountAsync(guildId, days, options);

        //public Task<int?> PruneAsync(Snowflake guildId, int days, bool computePruneCount = true, IRestRequestOptions options = null)
        //    => ApiClient.BeginGuildPruneAsync(guildId, days, computePruneCount, options);

        //public async Task<IReadOnlyList<RestGuildVoiceRegion>> GetVoiceRegionsAsync(Snowflake guildId, IRestRequestOptions options = null)
        //{
        //    var models = await ApiClient.GetGuildVoiceRegionsAsync(guildId, options).ConfigureAwait(false);
        //    return models.ToReadOnlyList((this, guildId), (x, tuple) =>
        //    {
        //        var (@this, guildId) = tuple;
        //        return new RestGuildVoiceRegion(@this, guildId, x);
        //    });
        //}

        //public async Task<IReadOnlyList<RestInvite>> GetGuildInvitesAsync(Snowflake guildId, IRestRequestOptions options = null)
        //{
        //    var models = await ApiClient.GetGuildInvitesAsync(guildId, options).ConfigureAwait(false);
        //    return models.ToReadOnlyList(this, (x, @this) => new RestInvite(@this, x));
        //}

        //public async Task<RestWidget> GetWidgetAsync(Snowflake guildId, IRestRequestOptions options = null)
        //{
        //    var model = await ApiClient.GetGuildEmbedAsync(guildId, options).ConfigureAwait(false);
        //    return new RestWidget(this, model, guildId);
        //}

        //public async Task<RestWidget> ModifyWidgetAsync(Snowflake guildId, Action<ModifyWidgetProperties> action, IRestRequestOptions options = null)
        //{
        //    var model = await InternalModifyWidgetAsync(guildId, action, options).ConfigureAwait(false);
        //    return new RestWidget(this, model, guildId);
        //}

        //internal async Task<WidgetModel> InternalModifyWidgetAsync(Snowflake guildId, Action<ModifyWidgetProperties> action, IRestRequestOptions options = null)
        //{
        //    if (action == null)
        //        throw new ArgumentNullException(nameof(action));

        //    var properties = new ModifyWidgetProperties();
        //    action(properties);
        //    return await ApiClient.ModifyGuildEmbedAsync(guildId, properties, options).ConfigureAwait(false);
        //}

        //public Task<string> GetVanityInviteAsync(Snowflake guildId, IRestRequestOptions options = null)
        //    => ApiClient.GetGuildVanityUrlAsync(guildId, options);

        //public async Task<RestPreview> GetPreviewAsync(Snowflake guildId, IRestRequestOptions options = null)
        //{
        //    var model = await ApiClient.GetGuildPreviewAsync(guildId, options).ConfigureAwait(false);
        //    return new RestPreview(this, model);
        //}
    }
}