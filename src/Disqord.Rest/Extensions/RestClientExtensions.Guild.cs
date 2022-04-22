using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Http;
using Disqord.Models;
using Disqord.Rest.Api;
using Disqord.Rest.Pagination;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        // TODO: create guild?

        public static async Task<IGuild> FetchGuildAsync(this IRestClient client,
            Snowflake guildId, bool? withCounts = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var model = await client.ApiClient.FetchGuildAsync(guildId, withCounts, options, cancellationToken).ConfigureAwait(false);
                return new TransientGuild(client, model);
            }
            catch (RestApiException ex) when (ex.StatusCode == HttpResponseStatusCode.NotFound)
            {
                return null;
            }
        }

        public static async Task<IGuild> ModifyGuildAsync(this IRestClient client,
            Snowflake guildId, Action<ModifyGuildActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(action);

            var properties = new ModifyGuildActionProperties();
            action(properties);
            var content = new ModifyGuildJsonRestRequestContent
            {
                Name = properties.Name,
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
                Description = properties.Description,
                PremiumProgressBarEnabled = properties.IsBoostProgressBarEnabled
            };

            var model = await client.ApiClient.ModifyGuildAsync(guildId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuild(client, model);
        }

        public static Task DeleteGuildAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.DeleteGuildAsync(guildId, options, cancellationToken);
        }

        public static async Task<IReadOnlyList<IGuildChannel>> FetchChannelsAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchGuildChannelsAsync(guildId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => TransientGuildChannel.Create(client, x));
        }

        public static async Task<ITextChannel> CreateTextChannelAsync(this IRestClient client,
            Snowflake guildId, string name, Action<CreateTextChannelActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.InternalCreateGuildChannelAsync(guildId, name, action, options, cancellationToken).ConfigureAwait(false);
            return new TransientTextChannel(client, model);
        }

        public static async Task<IVoiceChannel> CreateVoiceChannelAsync(this IRestClient client,
            Snowflake guildId, string name, Action<CreateVoiceChannelActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.InternalCreateGuildChannelAsync(guildId, name, action, options, cancellationToken).ConfigureAwait(false);
            return new TransientVoiceChannel(client, model);
        }

        public static async Task<ICategoryChannel> CreateCategoryChannelAsync(this IRestClient client,
            Snowflake guildId, string name, Action<CreateCategoryChannelActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.InternalCreateGuildChannelAsync(guildId, name, action, options, cancellationToken).ConfigureAwait(false);
            return new TransientCategoryChannel(client, model);
        }

        internal static Task<ChannelJsonModel> InternalCreateGuildChannelAsync<T>(this IRestClient client,
            Snowflake guildId, string name, Action<T> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
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
                content.ParentId = nestedProperties.CategoryId;

                if (properties is CreateTextChannelActionProperties textProperties)
                {
                    if (textProperties.Topic.HasValue && textProperties.Topic.Value != null && textProperties.Topic.Value.Length > 1024)
                        throw new ArgumentOutOfRangeException(nameof(CreateTextChannelActionProperties.Topic));

                    content.Type = ChannelType.Text;
                    content.Topic = textProperties.Topic;
                    content.RateLimitPerUser = Optional.Convert(textProperties.Slowmode, x => (int) x.TotalSeconds);
                    content.Nsfw = textProperties.IsNsfw;
                    content.DefaultAutoArchiveDuration = Optional.Convert(textProperties.DefaultAutomaticArchiveDuration, x => (int) x.TotalMinutes);
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

            return client.ApiClient.CreateGuildChannelAsync(guildId, content, options, cancellationToken);
        }

        public static Task ReorderChannelsAsync(this IRestClient client,
            Snowflake guildId, IReadOnlyDictionary<Snowflake, int> positions,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(positions);

            var contents = positions.Select(x => new ReorderJsonRestRequestContent
            {
                Id = x.Key,
                Position = x.Value
            }).ToArray();

            var content = new JsonObjectRestRequestContent<ReorderJsonRestRequestContent[]>(contents);
            return client.ApiClient.ReorderGuildChannelsAsync(guildId, content, options, cancellationToken);
        }

        public static async Task<IReadOnlyList<IThreadChannel>> FetchActiveThreadsAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchActiveThreadsAsync(guildId, options, cancellationToken).ConfigureAwait(false);
            return CreateThreads(client, model).Threads;
        }

        public static async Task<IMember> FetchMemberAsync(this IRestClient client,
            Snowflake guildId, Snowflake memberId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var model = await client.ApiClient.FetchMemberAsync(guildId, memberId, options, cancellationToken).ConfigureAwait(false);
                return new TransientMember(client, guildId, model);
            }
            catch (RestApiException ex) when (ex.ErrorModel.Code == RestApiErrorCode.UnknownMember)
            {
                return null;
            }
        }

        public static IPagedEnumerable<IMember> EnumerateMembers(this IRestClient client,
            Snowflake guildId, int limit, Snowflake? startFromId = null,
            IRestRequestOptions options = null)
        {
            Guard.IsGreaterThanOrEqualTo(limit, 0);

            return PagedEnumerable.Create((state, cancellationToken) =>
            {
                var (client, guildId, limit, startFromId, options) = state;
                return new FetchMembersPagedEnumerator(client, guildId, limit, startFromId, options, cancellationToken);
            }, (client, guildId, limit, startFromId, options));
        }

        public static Task<IReadOnlyList<IMember>> FetchMembersAsync(this IRestClient client,
            Snowflake guildId, int limit = Discord.Limits.Rest.FetchMembersPageSize, Snowflake? startFromId = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsGreaterThanOrEqualTo(limit, 0);

            if (limit == 0)
                return Task.FromResult(ReadOnlyList<IMember>.Empty);

            if (limit <= 1000)
                return client.InternalFetchMembersAsync(guildId, limit, startFromId, options, cancellationToken);

            var enumerator = client.EnumerateMembers(guildId, limit, startFromId, options);
            return enumerator.FlattenAsync(cancellationToken);
        }

        internal static async Task<IReadOnlyList<IMember>> InternalFetchMembersAsync(this IRestClient client,
            Snowflake guildId, int limit, Snowflake? startFromId,
            IRestRequestOptions options, CancellationToken cancellationToken)
        {
            var models = await client.ApiClient.FetchMembersAsync(guildId, limit, startFromId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), (x, state) =>
            {
                var (client, guildId) = state;
                return new TransientMember(client, guildId, x);
            });
        }

        public static async Task<IReadOnlyList<IMember>> SearchMembersAsync(this IRestClient client,
            Snowflake guildId, string query, int limit = Discord.Limits.Rest.FetchMembersPageSize,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.SearchMembersAsync(guildId, query, limit, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), static (x, state) =>
            {
                var (client, guildId) = state;
                return new TransientMember(client, guildId, x);
            });
        }

        public static async Task<IMember> AddMemberAsync(this IRestClient client,
            Snowflake guildId, Snowflake userId, BearerToken token,
            Action<AddMemberActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var content = new AddMemberJsonRestRequestContent(token.RawValue);
            if (action != null)
            {
                var properties = new AddMemberActionProperties();
                action(properties);
                content.Nick = properties.Nick;
                content.Roles = Optional.Convert(properties.RoleIds, roleIds => roleIds.ToArray());
                content.Mute = properties.IsMuted;
                content.Deaf = properties.IsDeafened;
            }

            var model = await client.ApiClient.AddMemberAsync(guildId, userId, content, options, cancellationToken);
            return new TransientMember(client, guildId, model);
        }

        public static async Task<IMember> ModifyMemberAsync(this IRestClient client,
            Snowflake guildId, Snowflake memberId, Action<ModifyMemberActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var content = action.ToContent(out var nick);

            if (nick.HasValue && client.ApiClient.Token is BotToken botToken)
            {
                if (memberId == botToken.Id)
                {
                    await client.ModifyCurrentMemberAsync(guildId, x => x.Nick = nick, options, cancellationToken).ConfigureAwait(false);
                    content.Nick = Optional<string>.Empty;
                }
            }

            var model = await client.ApiClient.ModifyMemberAsync(guildId, memberId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientMember(client, guildId, model);
        }

        public static async Task<IMember> ModifyCurrentMemberAsync(this IRestClient client,
            Snowflake guildId, Action<ModifyCurrentMemberActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var content = action.ToContent();
            var model = await client.ApiClient.ModifyCurrentMemberAsync(guildId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientMember(client, guildId, model);
        }

        [Obsolete("Use ModifyCurrentMemberAsync() instead.")]
        public static Task SetCurrentMemberNickAsync(this IRestClient client,
            Snowflake guildId, string nick,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var content = new SetOwnNickJsonRestRequestContent
            {
                Nick = nick
            };

            return client.ApiClient.SetOwnNickAsync(guildId, content, options, cancellationToken);
        }

        public static Task GrantRoleAsync(this IRestClient client,
            Snowflake guildId, Snowflake memberId, Snowflake roleId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.GrantRoleAsync(guildId, memberId, roleId, options, cancellationToken);
        }

        public static Task RevokeRoleAsync(this IRestClient client,
            Snowflake guildId, Snowflake memberId, Snowflake roleId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.RevokeRoleAsync(guildId, memberId, roleId, options, cancellationToken);
        }

        public static Task KickMemberAsync(this IRestClient client,
            Snowflake guildId, Snowflake memberId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.KickMemberAsync(guildId, memberId, options, cancellationToken);
        }

        public static IPagedEnumerable<IBan> EnumerateBans(this IRestClient client,
            Snowflake guildId,
            int limit, RetrievalDirection direction = RetrievalDirection.After, Snowflake? startFromId = null,
            IRestRequestOptions options = null)
        {
            Guard.IsGreaterThanOrEqualTo(limit, 0);

            return PagedEnumerable.Create((state, cancellationToken) =>
            {
                var (client, guildId, limit, direction, startFromId, options) = state;
                return new FetchBansEnumerator(client, guildId, limit, direction, startFromId, options, cancellationToken);
            }, (client, guildId, limit, direction, startFromId, options));
        }

        public static Task<IReadOnlyList<IBan>> FetchBansAsync(this IRestClient client,
            Snowflake guildId,
            int limit = Discord.Limits.Rest.FetchBansPageSize, RetrievalDirection direction = RetrievalDirection.After, Snowflake? startFromId = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsGreaterThanOrEqualTo(limit, 0);

            if (limit == 0)
                return Task.FromResult(ReadOnlyList<IBan>.Empty);

            if (limit <= Discord.Limits.Rest.FetchBansPageSize)
                return client.InternalFetchBansAsync(guildId, limit, direction, startFromId, options, cancellationToken);

            var enumerable = client.EnumerateBans(guildId, limit, direction, startFromId, options);
            return enumerable.FlattenAsync(cancellationToken);
        }

        internal static async Task<IReadOnlyList<IBan>> InternalFetchBansAsync(this IRestClient client,
            Snowflake guildId,
            int limit, RetrievalDirection direction, Snowflake? startFromId,
            IRestRequestOptions options, CancellationToken cancellationToken)
        {
            var models = await client.ApiClient.FetchBansAsync(guildId, limit, direction, startFromId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), (model, state) =>
            {
                var (client, guildId) = state;
                return new TransientBan(client, guildId, model);
            });
        }

        public static async Task<IBan> FetchBanAsync(this IRestClient client,
            Snowflake guildId, Snowflake userId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var model = await client.ApiClient.FetchBanAsync(guildId, userId, options, cancellationToken).ConfigureAwait(false);
                return new TransientBan(client, guildId, model);
            }
            catch (RestApiException ex) when (ex.ErrorModel.Code == RestApiErrorCode.UnknownBan)
            {
                return null;
            }
        }

        public static Task CreateBanAsync(this IRestClient client,
            Snowflake guildId, Snowflake userId, string reason = null, int? deleteMessageDays = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var content = new CreateBanJsonRestRequestContent
            {
                DeleteMessageDays = Optional.FromNullable(deleteMessageDays),
                Reason = Optional.FromNullable(reason)
            };

            return client.ApiClient.CreateBanAsync(guildId, userId, content, options, cancellationToken);
        }

        public static Task DeleteBanAsync(this IRestClient client,
            Snowflake guildId, Snowflake userId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.DeleteBanAsync(guildId, userId, options, cancellationToken);
        }

        public static async Task<IReadOnlyList<IRole>> FetchRolesAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchRolesAsync(guildId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), static (x, state) =>
            {
                var (client, guildId) = state;
                return new TransientRole(client, guildId, x);
            });
        }

        public static async Task<IRole> CreateRoleAsync(this IRestClient client,
            Snowflake guildId, Action<CreateRoleActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var properties = new CreateRoleActionProperties();
            action?.Invoke(properties);

            if (properties.UnicodeEmoji.HasValue && properties.UnicodeEmoji.Value is LocalCustomEmoji)
                throw new ArgumentException("The role's emoji must be a Unicode emoji.");

            var content = new CreateRoleJsonRestRequestContent
            {
                Name = properties.Name,
                Permissions = Optional.Convert(properties.Permissions, permissions => permissions.RawValue),
                Color = Optional.Convert(properties.Color, color => color?.RawValue ?? 0),
                Hoist = properties.IsHoisted,
                Icon = properties.Icon,
                Mentionable = properties.IsMentionable,
                UnicodeEmoji = Optional.Convert(properties.UnicodeEmoji, emoji => emoji.Name)
            };

            var model = await client.ApiClient.CreateRoleAsync(guildId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientRole(client, guildId, model);
        }

        public static async Task<IReadOnlyList<IRole>> ReorderRolesAsync(this IRestClient client,
            Snowflake guildId, IReadOnlyDictionary<Snowflake, int> positions,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(positions);

            var contents = positions.Select(x => new ReorderJsonRestRequestContent
            {
                Id = x.Key,
                Position = x.Value
            }).ToArray();

            var content = new JsonObjectRestRequestContent<ReorderJsonRestRequestContent[]>(contents);
            var models = await client.ApiClient.ReorderRolesAsync(guildId, content, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), static (x, state) =>
            {
                var (client, guildId) = state;
                return new TransientRole(client, guildId, x);
            });
        }

        public static async Task<IRole> ModifyRoleAsync(this IRestClient client,
            Snowflake guildId, Snowflake roleId, Action<ModifyRoleActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var content = action.ToContent(out var position);
            if (position.HasValue)
            {
                await client.ReorderRolesAsync(guildId, new Dictionary<Snowflake, int>
                {
                    [roleId] = position.Value
                }, options, cancellationToken).ConfigureAwait(false);
            }

            var model = await client.ApiClient.ModifyRoleAsync(guildId, roleId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientRole(client, guildId, model);
        }

        public static Task DeleteRoleAsync(this IRestClient client,
            Snowflake guildId, Snowflake roleId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.DeleteRoleAsync(guildId, roleId, options, cancellationToken);
        }

        public static async Task<int> FetchPruneGuildCountAsync(this IRestClient client,
            Snowflake guildId, int days, IEnumerable<Snowflake> roleIds = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchPruneGuildCountAsync(guildId, days, roleIds?.ToArray(), options, cancellationToken).ConfigureAwait(false);
            return model.Pruned.Value;
        }

        public static async Task<int?> PruneGuildAsync(this IRestClient client,
            Snowflake guildId, int days, bool computePruneCount = true, IEnumerable<Snowflake> roleIds = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.PruneGuildAsync(guildId, days, computePruneCount, roleIds?.ToArray(), options, cancellationToken).ConfigureAwait(false);
            return model.Pruned;
        }

        public static async Task<IReadOnlyList<IGuildVoiceRegion>> FetchGuildVoiceRegionsAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchGuildVoiceRegionsAsync(guildId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), static (x, state) =>
            {
                var (client, guildId) = state;
                return new TransientGuildVoiceRegion(client, guildId, x);
            });
        }

        public static async Task<IReadOnlyList<IInvite>> FetchGuildInvitesAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchGuildInvitesAsync(guildId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => TransientInvite.Create(client, x));
        }

        public static async Task<IReadOnlyList<IIntegration>> FetchIntegrationsAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchIntegrationsAsync(guildId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), static (x, state) =>
            {
                var (client, guildId) = state;
                return new TransientIntegration(client, guildId, x);
            });
        }

        public static Task DeleteIntegrationAsync(this IRestClient client,
            Snowflake guildId, Snowflake integrationId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.DeleteIntegrationAsync(guildId, integrationId, options, cancellationToken);
        }

        public static async Task<IGuildWidget> FetchWidgetAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchGuildWidgetAsync(guildId, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildWidget(client, guildId, model);
        }

        public static async Task<IGuildWidget> ModifyWidgetAsync(this IRestClient client,
            Snowflake guildId, Action<ModifyWidgetActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var properties = new ModifyWidgetActionProperties();
            action(properties);

            var content = new ModifyGuildWidgetSettingsJsonRestRequestContent
            {
                Enabled = properties.IsEnabled,
                ChannelId = properties.ChannelId
            };

            var model = await client.ApiClient.ModifyGuildWidgetAsync(guildId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildWidget(client, guildId, model);
        }

        public static async Task<IVanityInvite> FetchVanityInviteAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchGuildVanityInviteAsync(guildId, options, cancellationToken).ConfigureAwait(false);
            return new TransientVanityInvite(client, guildId, model);
        }

        public static async Task<IGuildPreview> FetchPreviewAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchGuildPreviewAsync(guildId, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildPreview(client, model);
        }

        public static async Task<IGuildWelcomeScreen> FetchGuildWelcomeScreenAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchGuildWelcomeScreenAsync(guildId, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildWelcomeScreen(client, guildId, model);
        }

        public static async Task<IGuildWelcomeScreen> ModifyGuildWelcomeScreenAsync(this IRestClient client,
            Snowflake guildId, Action<ModifyWelcomeScreenActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(action);

            var properties = new ModifyWelcomeScreenActionProperties();
            action(properties);

            var content = new ModifyWelcomeScreenJsonRestRequestContent()
            {
                Enabled = properties.IsEnabled,
                Description = properties.Description,
                WelcomeChannels = Optional.Convert(properties.Channels, x => x.Select(x => x.ToModel()).ToArray())
            };

            var model = await client.ApiClient.ModifyGuildWelcomeScreenAsync(guildId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildWelcomeScreen(client, guildId, model);
        }

        public static Task ModifyCurrentMemberVoiceStateAsync(this IRestClient client,
            Snowflake guildId, Snowflake channelId, Action<ModifyCurrentMemberVoiceStateActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var properties = new ModifyCurrentMemberVoiceStateActionProperties();
            action(properties);

            var content = new ModifyCurrentMemberVoiceStateJsonRestRequestContent
            {
                ChannelId = channelId,
                Suppress = properties.IsSuppressed,
                RequestToSpeakTimestamp = properties.RequestedToSpeakAt
            };

            return client.ApiClient.ModifyCurrentMemberVoiceStateAsync(guildId, content, options, cancellationToken);
        }

        public static Task ModifyMemberVoiceStateAsync(this IRestClient client,
            Snowflake guildId, Snowflake memberId, Snowflake channelId, Action<ModifyMemberVoiceStateActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var properties = new ModifyMemberVoiceStateActionProperties();
            action(properties);

            var content = new ModifyMemberVoiceStateJsonRestRequestContent
            {
                ChannelId = channelId,
                Suppress = properties.IsSuppressed,
            };

            return client.ApiClient.ModifyMemberVoiceStateAsync(guildId, memberId, content, options, cancellationToken);
        }
    }
}
