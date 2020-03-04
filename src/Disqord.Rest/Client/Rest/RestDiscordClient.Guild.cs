using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public async Task<RestGuild> CreateGuildAsync(
            string name, string voiceRegionId = null, Stream icon = null, VerificationLevel verificationLevel = default,
            DefaultNotificationLevel defaultNotificationLevel = default, ContentFilterLevel contentFilterLevel = default,
            RestRequestOptions options = null)
        {
            var model = await ApiClient.CreateGuildAsync(name, voiceRegionId, icon, verificationLevel, defaultNotificationLevel, contentFilterLevel, options).ConfigureAwait(false);
            return new RestGuild(this, model);
        }

        public async Task<RestGuild> GetGuildAsync(Snowflake guildId, RestRequestOptions options = null)
        {
            try
            {
                var model = await ApiClient.GetGuildAsync(guildId, options).ConfigureAwait(false);
                return new RestGuild(this, model);
            }
            catch (DiscordHttpException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<RestGuild> ModifyGuildAsync(Snowflake guildId, Action<ModifyGuildProperties> action, RestRequestOptions options = null)
        {
            var model = await InternalModifyGuildAsync(guildId, action, options).ConfigureAwait(false);
            return new RestGuild(this, model);
        }

        internal async Task<GuildModel> InternalModifyGuildAsync(Snowflake guildId, Action<ModifyGuildProperties> action, RestRequestOptions options = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyGuildProperties();
            action(properties);
            return await ApiClient.ModifyGuildAsync(guildId, properties, options).ConfigureAwait(false);
        }

        public Task DeleteGuildAsync(Snowflake guildId, RestRequestOptions options = null)
            => ApiClient.DeleteGuildAsync(guildId, options);

        public async Task<IReadOnlyList<RestGuildChannel>> GetChannelsAsync(Snowflake guildId, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetGuildChannelsAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(this, (x, @this) => RestGuildChannel.Create(@this, x));
        }

        public async Task<RestTextChannel> CreateTextChannelAsync(Snowflake guildId, string name, Action<CreateTextChannelProperties> action = null, RestRequestOptions options = null)
        {
            var model = await InternalCreateGuildChannelAsync(guildId, name, action, options).ConfigureAwait(false);
            return new RestTextChannel(this, model);
        }

        public async Task<RestVoiceChannel> CreateVoiceChannelAsync(Snowflake guildId, string name, Action<CreateVoiceChannelProperties> action = null, RestRequestOptions options = null)
        {
            var model = await InternalCreateGuildChannelAsync(guildId, name, action, options).ConfigureAwait(false);
            return new RestVoiceChannel(this, model);
        }

        public async Task<RestCategoryChannel> CreateCategoryChannelAsync(Snowflake guildId, string name, Action<CreateCategoryChannelProperties> action = null, RestRequestOptions options = null)
        {
            var model = await InternalCreateGuildChannelAsync(guildId, name, action, options).ConfigureAwait(false);
            return new RestCategoryChannel(this, model);
        }

        internal async Task<ChannelModel> InternalCreateGuildChannelAsync<T>(Snowflake guildId, string name, Action<T> action, RestRequestOptions options = null)
            where T : CreateGuildChannelProperties
        {
            // Can't use the new() generic constraint because the constructors are internal.
            // Can't use the generic CreateInstance either *because*.
            var properties = (T) Activator.CreateInstance(typeof(T), true);
            action?.Invoke(properties);
            return await ApiClient.CreateGuildChannelAsync(guildId, name, properties, options).ConfigureAwait(false);
        }

        public Task ReorderChannelsAsync(Snowflake guildId, IReadOnlyDictionary<Snowflake, int> positions, RestRequestOptions options = null)
        {
            if (positions == null)
                throw new ArgumentNullException(nameof(positions));

            return ApiClient.ModifyGuildChannelPositionsAsync(guildId, positions, options);
        }

        public async Task<RestMember> GetMemberAsync(Snowflake guildId, Snowflake memberId, RestRequestOptions options = null)
        {
            try
            {
                var model = await ApiClient.GetMemberAsync(guildId, memberId, options).ConfigureAwait(false);
                return new RestMember(this, guildId, model);
            }
            catch (DiscordHttpException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public RestRequestEnumerable<RestMember> GetMembersEnumerable(Snowflake guildId, int limit, Snowflake? startFromId = null, RestRequestOptions options = null)
            => new RestRequestEnumerable<RestMember>(new RestMembersRequestEnumerator(this, guildId, limit, startFromId, options));

        public Task<IReadOnlyList<RestMember>> GetMembersAsync(Snowflake guildId, int limit = 1000, Snowflake? startFromId = null, RestRequestOptions options = null)
        {
            if (limit == 0)
                return Task.FromResult(ReadOnlyList<RestMember>.Empty);

            if (limit <= 1000)
                return InternalGetMembersAsync(guildId, limit, startFromId, options);

            var enumerator = GetMembersEnumerable(guildId, limit, startFromId, options);
            return enumerator.FlattenAsync();
        }

        internal async Task<IReadOnlyList<RestMember>> InternalGetMembersAsync(Snowflake guildId, int limit, Snowflake? startFromId, RestRequestOptions options)
        {
            var models = await ApiClient.ListGuildMembersAsync(guildId, limit, startFromId ?? 0, options).ConfigureAwait(false);
            return models.ToReadOnlyList((this, guildId), (x, tuple) =>
            {
                var (@this, guildId) = tuple;
                return new RestMember(@this, guildId, x);
            });
        }

        public Task ModifyOwnNickAsync(Snowflake guildId, string nick, RestRequestOptions options = null)
            => ApiClient.ModifyCurrentUserNickAsync(guildId, nick, options);

        public async Task ModifyMemberAsync(Snowflake guildId, Snowflake memberId, Action<ModifyMemberProperties> action, RestRequestOptions options = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyMemberProperties();
            action(properties);

            if (properties.Nick.HasValue)
            {
                var currentUser = await CurrentUser.GetAsync(options).ConfigureAwait(false);
                if (currentUser.Id == memberId)
                {
                    await ApiClient.ModifyCurrentUserNickAsync(guildId, properties.Nick.Value, options).ConfigureAwait(false);
                    properties.Nick = Optional<string>.Empty;
                }
            }

            if (properties.HasValues)
                await ApiClient.ModifyGuildMemberAsync(guildId, memberId, properties, options).ConfigureAwait(false);
        }

        public Task GrantRoleAsync(Snowflake guildId, Snowflake memberId, Snowflake roleId, RestRequestOptions options = null)
            => ApiClient.AddGuildMemberRoleAsync(guildId, memberId, roleId, options);

        public Task RevokeRoleAsync(Snowflake guildId, Snowflake memberId, Snowflake roleId, RestRequestOptions options = null)
            => ApiClient.DeleteGuildMemberRoleAsync(guildId, memberId, roleId, options);

        public Task KickMemberAsync(Snowflake guildId, Snowflake memberId, RestRequestOptions options = null)
            => ApiClient.RemoveMemberAsync(guildId, memberId, options);

        public async Task<IReadOnlyList<RestBan>> GetBansAsync(Snowflake guildId, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetGuildBansAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList((this, guildId), (x, tuple) =>
            {
                var (@this, guildId) = tuple;
                return new RestBan(@this, guildId, x);
            });
        }

        public async Task<RestBan> GetBanAsync(Snowflake guildId, Snowflake userId, RestRequestOptions options = null)
        {
            try
            {
                var model = await ApiClient.GetGuildBanAsync(guildId, userId, options).ConfigureAwait(false);
                return new RestBan(this, guildId, model);
            }
            catch (DiscordHttpException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public Task BanMemberAsync(Snowflake guildId, Snowflake memberId, string reason = null, int? deleteMessageDays = null, RestRequestOptions options = null)
            => ApiClient.CreateGuildBanAsync(guildId, memberId, reason, deleteMessageDays, options);

        public Task UnbanMemberAsync(Snowflake guildId, Snowflake userId, RestRequestOptions options = null)
            => ApiClient.RemoveGuildBanAsync(guildId, userId, options);

        public async Task<IReadOnlyList<RestRole>> GetRolesAsync(Snowflake guildId, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetGuildRolesAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList((this, guildId), (x, tuple) =>
            {
                var (@this, guildId) = tuple;
                return new RestRole(@this, guildId, x);
            });
        }

        public async Task<RestRole> CreateRoleAsync(Snowflake guildId, Action<CreateRoleProperties> action = null, RestRequestOptions options = null)
        {
            var properties = new CreateRoleProperties();
            action?.Invoke(properties);
            var model = await ApiClient.CreateGuildRoleAsync(guildId, properties, options).ConfigureAwait(false);
            return new RestRole(this, guildId, model);
        }

        public async Task<IReadOnlyList<RestRole>> ReorderRolesAsync(Snowflake guildId, IReadOnlyDictionary<Snowflake, int> positions, RestRequestOptions options = null)
        {
            if (positions == null)
                throw new ArgumentNullException(nameof(positions));

            var models = await ApiClient.ModifyGuildRolePositionsAsync(guildId, positions, options).ConfigureAwait(false);
            return models.ToReadOnlyList((this, guildId), (x, tuple) =>
            {
                var (@this, guildId) = tuple;
                return new RestRole(@this, guildId, x);
            });
        }

        public async Task<RestRole> ModifyRoleAsync(Snowflake guildId, Snowflake roleId, Action<ModifyRoleProperties> action, RestRequestOptions options = null)
        {
            var model = await InternalModifyRoleAsync(guildId, roleId, action, options).ConfigureAwait(false);
            return new RestRole(this, guildId, model);
        }

        internal async Task<RoleModel> InternalModifyRoleAsync(Snowflake guildId, Snowflake roleId, Action<ModifyRoleProperties> action, RestRequestOptions options = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyRoleProperties();
            action(properties);
            if (properties.Position.HasValue)
            {
                await ReorderRolesAsync(guildId, new Dictionary<Snowflake, int>
                {
                    [roleId] = properties.Position.Value
                }, options).ConfigureAwait(false);
            }

            return await ApiClient.ModifyGuildRoleAsync(guildId, roleId, properties, options).ConfigureAwait(false);
        }

        public Task DeleteRoleAsync(Snowflake guildId, Snowflake roleId, RestRequestOptions options = null)
            => ApiClient.DeleteGuildRoleAsync(guildId, roleId, options);

        public Task<int> GetPruneCountAsync(Snowflake guildId, int days, RestRequestOptions options = null)
            => ApiClient.GetGuildPruneCountAsync(guildId, days, options);

        public Task<int?> PruneAsync(Snowflake guildId, int days, bool computePruneCount = true, RestRequestOptions options = null)
            => ApiClient.BeginGuildPruneAsync(guildId, days, computePruneCount, options);

        public async Task<IReadOnlyList<RestGuildVoiceRegion>> GetVoiceRegionsAsync(Snowflake guildId, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetGuildVoiceRegionsAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList((this, guildId), (x, tuple) =>
            {
                var (@this, guildId) = tuple;
                return new RestGuildVoiceRegion(@this, guildId, x);
            });
        }

        public async Task<IReadOnlyList<RestInvite>> GetGuildInvitesAsync(Snowflake guildId, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetGuildInvitesAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(this, (x, @this) => new RestInvite(@this, x));
        }

        public async Task<RestWidget> GetWidgetAsync(Snowflake guildId, RestRequestOptions options = null)
        {
            var model = await ApiClient.GetGuildEmbedAsync(guildId, options).ConfigureAwait(false);
            return new RestWidget(this, model, guildId);
        }

        public async Task<RestWidget> ModifyWidgetAsync(Snowflake guildId, Action<ModifyWidgetProperties> action, RestRequestOptions options = null)
        {
            var model = await InternalModifyWidgetAsync(guildId, action, options).ConfigureAwait(false);
            return new RestWidget(this, model, guildId);
        }

        internal async Task<WidgetModel> InternalModifyWidgetAsync(Snowflake guildId, Action<ModifyWidgetProperties> action, RestRequestOptions options = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyWidgetProperties();
            action(properties);
            return await ApiClient.ModifyGuildEmbedAsync(guildId, properties, options).ConfigureAwait(false);
        }

        public Task<string> GetVanityInviteAsync(Snowflake guildId, RestRequestOptions options = null)
            => ApiClient.GetGuildVanityUrlAsync(guildId, options);

        public async Task<RestPreview> GetPreviewAsync(Snowflake guildId, RestRequestOptions options = null)
        {
            var model = await ApiClient.GetGuildPreviewAsync(guildId, options).ConfigureAwait(false);
            return new RestPreview(this, model);
        }
    }
}
