using System;
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
                DiscoverySplash = properties.DiscoverySplash,
                Banner = properties.Banner,
                SystemChannelId = properties.SystemChannelId,
                SystemChannelFlags = properties.SystemChannelFlags,
                RulesChannelId = properties.RulesChannelId,
                PublicUpdatesChannelId = properties.PublicUpdatesChannelId,
                PreferredLocale = Optional.Convert(properties.PreferredLocale, x => x.Name),
                Features = Optional.Convert(properties.Features, x => x.ToArray()),
                Description = properties.Description
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
                Position = properties.Position,
                PermissionOverwrites = Optional.Convert(properties.Overwrites, x => x.Select(x => x.ToModel()).ToArray())
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
                    content.UserLimit = voiceProperties.MemberLimit;
                    content.RtcRegion = voiceProperties.Region;
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

            var contents = positions.Select(x => new ReorderJsonRestRequestContent
            {
                Id = x.Key,
                Position = x.Value
            }).ToArray();
            var content = new JsonObjectRestRequestContent<ReorderJsonRestRequestContent[]>(contents);
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

        public static async Task<IReadOnlyList<IMember>> SearchMembersAsync(this IRestClient client, Snowflake guildId, string query, int limit = 1000, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.SearchMembersAsync(guildId, query, limit, options).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), static (x, tuple) =>
            {
                var (client, guildId) = tuple;
                return new TransientMember(client, guildId, x);
            });
        }

        // TODO: add member

        public static Task SetCurrentMemberNickAsync(this IRestClient client, Snowflake guildId, string nick, IRestRequestOptions options = null)
        {
            var content = new SetOwnNickJsonRestRequestContent
            {
                Nick = nick
            };
            return client.ApiClient.SetOwnNickAsync(guildId, content, options);
        }

        public static async Task<IMember> ModifyMemberAsync(this IRestClient client, Snowflake guildId, Snowflake memberId, Action<ModifyMemberActionProperties> action, IRestRequestOptions options = null)
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
            var model = await client.ApiClient.ModifyMemberAsync(guildId, memberId, content, options).ConfigureAwait(false);
            return new TransientMember(client, guildId, model);
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
            return models.ToReadOnlyList((client, guildId), static (x, tuple) =>
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

        public static Task CreateBanAsync(this IRestClient client, Snowflake guildId, Snowflake userId, string reason = null, int? deleteMessageDays = null, IRestRequestOptions options = null)
        {
            var content = new CreateBanJsonRestRequestContent
            {
                DeleteMessageDays = Optional.FromNullable(deleteMessageDays),
                Reason = Optional.FromNullable(reason)
            };
            return client.ApiClient.CreateBanAsync(guildId, userId, content, options);
        }

        public static Task DeleteBanAsync(this IRestClient client, Snowflake guildId, Snowflake userId, IRestRequestOptions options = null)
            => client.ApiClient.DeleteBanAsync(guildId, userId, options);

        public static async Task<IReadOnlyList<IRole>> FetchRolesAsync(this IRestClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchRolesAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), static (x, tuple) =>
            {
                var (client, guildId) = tuple;
                return new TransientRole(client, guildId, x);
            });
        }

        public static async Task<IRole> CreateRoleAsync(this IRestClient client, Snowflake guildId, Action<CreateRoleActionProperties> action = null, IRestRequestOptions options = null)
        {
            var properties = new CreateRoleActionProperties();
            action?.Invoke(properties);
            var content = new CreateRoleJsonRestRequestContent
            {
                Name = properties.Name,
                Permissions = Optional.Convert(properties.Permissions, x => x.RawValue),
                Color = Optional.Convert(properties.Color, x => x?.RawValue ?? 0),
                Hoist = properties.IsHoisted,
                Mentionable = properties.IsMentionable
            };
            var model = await client.ApiClient.CreateRoleAsync(guildId, content, options).ConfigureAwait(false);
            return new TransientRole(client, guildId, model);
        }

        public static async Task<IReadOnlyList<IRole>> ReorderRolesAsync(this IRestClient client, Snowflake guildId, IReadOnlyDictionary<Snowflake, int> positions, IRestRequestOptions options = null)
        {
            if (positions == null)
                throw new ArgumentNullException(nameof(positions));

            var contents = positions.Select(x => new ReorderJsonRestRequestContent
            {
                Id = x.Key,
                Position = x.Value
            }).ToArray();
            var content = new JsonObjectRestRequestContent<ReorderJsonRestRequestContent[]>(contents);
            var models = await client.ApiClient.ReorderRolesAsync(guildId, content, options).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), static (x, tuple) =>
            {
                var (client, guildId) = tuple;
                return new TransientRole(client, guildId, x);
            });
        }

        public static async Task<IRole> ModifyRoleAsync(this IRestClient client, Snowflake guildId, Snowflake roleId, Action<ModifyRoleActionProperties> action, IRestRequestOptions options = null)
        {
            var properties = new ModifyRoleActionProperties();
            action?.Invoke(properties);
            if (properties.Position.HasValue)
            {
                await client.ReorderRolesAsync(guildId, new Dictionary<Snowflake, int>
                {
                    [roleId] = properties.Position.Value
                }, options).ConfigureAwait(false);
            }
            var content = new ModifyRoleJsonRestRequestContent
            {
                Name = properties.Name,
                Permissions = Optional.Convert(properties.Permissions, x => x.RawValue),
                Color = Optional.Convert(properties.Color, x => x?.RawValue ?? 0),
                Hoist = properties.IsHoisted,
                Mentionable = properties.IsMentionable
            };
            var model = await client.ApiClient.ModifyRoleAsync(guildId, roleId, content, options).ConfigureAwait(false);
            return new TransientRole(client, guildId, model);
        }

        public static Task DeleteRoleAsync(this IRestClient client, Snowflake guildId, Snowflake roleId, IRestRequestOptions options = null)
            => client.ApiClient.DeleteRoleAsync(guildId, roleId, options);

        public static async Task<int> FetchPruneGuildCountAsync(this IRestClient client, Snowflake guildId, int days, IEnumerable<Snowflake> roleIds = null, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchPruneGuildCountAsync(guildId, days, roleIds?.ToArray(), options).ConfigureAwait(false);
            return model.Pruned.Value;
        }

        public static async Task<int?> PruneGuildAsync(this IRestClient client, Snowflake guildId, int days, bool computePruneCount = true, IEnumerable<Snowflake> roleIds = null, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.PruneGuildAsync(guildId, days, computePruneCount, roleIds?.ToArray(), options).ConfigureAwait(false);
            return model.Pruned;
        }

        //public async Task<IReadOnlyList<RestGuildVoiceRegion>> GetVoiceRegionsAsync(Snowflake guildId, IRestRequestOptions options = null)
        //{
        //    var models = await ApiClient.GetGuildVoiceRegionsAsync(guildId, options).ConfigureAwait(false);
        //    return models.ToReadOnlyList((this, guildId), (x, tuple) =>
        //    {
        //        var (@this, guildId) = tuple;
        //        return new RestGuildVoiceRegion(@this, guildId, x);
        //    });
        //}

        public static async Task<IReadOnlyList<IInvite>> FetchGuildInvitesAsync(this IRestClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchGuildInvitesAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => new TransientInvite(client, x));
        }

        public static async Task<IReadOnlyList<IIntegration>> FetchIntegrationsAsync(this IRestClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchIntegrationsAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), static(x, tuple) =>
            {
                var (client, guildId) = tuple;
                return new TransientIntegration(client, guildId, x);
            });
        }

        public static Task DeleteIntegrationAsync(this IRestClient client, Snowflake guildId, Snowflake integrationId, IRestRequestOptions options = null)
            => client.ApiClient.DeleteIntegrationAsync(guildId, integrationId, options);

        public static async Task<IWidget> FetchWidgetAsync(this IRestClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchGuildWidgetAsync(guildId, options).ConfigureAwait(false);
            return new TransientWidget(client, model);
        }

        public static async Task<IWidget> ModifyWidgetAsync(this IRestClient client, Snowflake guildId, Action<ModifyWidgetActionProperties> action, IRestRequestOptions options = null)
        {
            var properties = new ModifyWidgetActionProperties();
            action?.Invoke(properties);

            var content = new ModifyGuildWidgetSettingsJsonRestRequestContent
            {
                Enabled = properties.IsEnabled,
                ChannelId = properties.ChannelId
            };
            
            var model = await client.ApiClient.ModifyGuildWidgetAsync(guildId, content, options).ConfigureAwait(false);
            return new TransientWidget(client, model);
        }

        //internal async Task<WidgetModel> InternalModifyWidgetAsync(Snowflake guildId, Action<ModifyWidgetProperties> action, IRestRequestOptions options = null)
        //{
        //    if (action == null)
        //        throw new ArgumentNullException(nameof(action));

        //    var properties = new ModifyWidgetProperties();
        //    action(properties);
        //    return await ApiClient.ModifyGuildEmbedAsync(guildId, properties, options).ConfigureAwait(false);
        //}

        public static async Task<IVanityInvite> FetchVanityInviteAsync(this IRestClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchGuildVanityInviteAsync(guildId, options);
            return new TransientVanityInvite(client, model);
        }

        public static async Task<IGuildPreview> FetchPreviewAsync(this IRestClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchGuildPreviewAsync(guildId, options).ConfigureAwait(false);
            return new TransientGuildPreview(client, model);
        }
    }
}
