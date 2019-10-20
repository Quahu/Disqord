using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public async Task<RestGuild> CreateGuildAsync(
            string name, string voiceRegionId = null, LocalAttachment icon = null, VerificationLevel verificationLevel = default,
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
            catch (HttpDiscordException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
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
            return models.Select(x => RestGuildChannel.Create(this, x)).ToImmutableArray();
        }

        //public async Task<RestTextChannel> CreateTextChannelAsync(Snowflake  guildId, string name,
        //    string topic = null, int slowmode = 0, bool isNSFW = false,
        //    IEnumerable<LocalOverwrite> overwrites = null, int? position = null, Snowflake ? categoryId = null,
        //    RestRequestOptions options = null)
        //{
        //    var model = await ApiClient.CreateGuildChannelAsync();
        //    return new RestTextChannel(this, model);
        //}


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
                return new RestMember(this, model, guildId);
            }
            catch (HttpDiscordException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public RestRequestEnumerator<RestMember> GetMembersEnumerator(Snowflake guildId, int limit, Snowflake? startFromId = null)
        {
            var enumerator = new RestRequestEnumerator<RestMember>();
            var remaining = limit;
            do
            {
                var amount = remaining > 1000 ? 1000 : remaining;
                remaining -= amount;
                enumerator.Enqueue(async (previous, options) =>
                {
                    var members = await InternalGetMembersAsync(guildId, amount, previous?.Count > 0 ? previous.Max(x => x.Id) : startFromId, options).ConfigureAwait(false);
                    if (members.Count < 1000)
                        enumerator.Cancel();

                    return members;
                });
            }
            while (remaining > 0);
            return enumerator;
        }

        public async Task<IReadOnlyList<RestMember>> GetMembersAsync(Snowflake guildId, int limit = 1000, Snowflake? startFromId = null, RestRequestOptions options = null)
        {
            if (limit == 0)
                return ImmutableArray<RestMember>.Empty;

            if (limit <= 1000)
                return await InternalGetMembersAsync(guildId, limit, startFromId, options).ConfigureAwait(false);

            var enumerator = GetMembersEnumerator(guildId, limit, startFromId);
            await using (enumerator.ConfigureAwait(false))
            {
                return await enumerator.FlattenAsync(options).ConfigureAwait(false);
            }
        }

        internal async Task<IReadOnlyList<RestMember>> InternalGetMembersAsync(Snowflake guildId, int limit, Snowflake? startFromId, RestRequestOptions options)
        {
            var models = await ApiClient.ListGuildMembersAsync(guildId, limit, startFromId ?? 0, options).ConfigureAwait(false);
            return models.Select(x => new RestMember(this, x, guildId)).ToImmutableArray();
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
                var currentUser = await CurrentUser.GetOrDownloadAsync(options).ConfigureAwait(false);
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
            return models.Select(x => new RestBan(this, x, guildId)).ToImmutableArray();
        }

        public async Task<RestBan> GetBanAsync(Snowflake guildId, Snowflake userId, RestRequestOptions options = null)
        {
            try
            {
                var model = await ApiClient.GetGuildBanAsync(guildId, userId, options).ConfigureAwait(false);
                return new RestBan(this, model, guildId);
            }
            catch (HttpDiscordException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
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
            return models.Select(x => new RestRole(this, x, guildId)).ToImmutableArray();
        }

        public async Task<RestRole> CreateRoleAsync(Snowflake guildId, Action<CreateRoleProperties> action = null, RestRequestOptions options = null)
        {
            var properties = new CreateRoleProperties();
            action?.Invoke(properties);
            var model = await ApiClient.CreateGuildRoleAsync(guildId, properties, options).ConfigureAwait(false);
            return new RestRole(this, model, guildId);
        }

        public async Task<IReadOnlyList<RestRole>> ReorderRolesAsync(Snowflake guildId, IReadOnlyDictionary<Snowflake, int> positions, RestRequestOptions options = null)
        {
            if (positions == null)
                throw new ArgumentNullException(nameof(positions));

            var models = await ApiClient.ModifyGuildRolePositionsAsync(guildId, positions, options).ConfigureAwait(false);
            return models.Select(x => new RestRole(this, x, guildId)).ToImmutableArray();
        }

        public async Task<RestRole> ModifyRoleAsync(Snowflake guildId, Snowflake roleId, Action<ModifyRoleProperties> action, RestRequestOptions options = null)
        {
            var model = await InternalModifyRoleAsync(guildId, roleId, action, options).ConfigureAwait(false);
            return new RestRole(this, model, guildId);
        }

        internal async Task<RoleModel> InternalModifyRoleAsync(Snowflake guildId, Snowflake roleId, Action<ModifyRoleProperties> action, RestRequestOptions options = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyRoleProperties();
            action(properties);
            if (properties.Position.HasValue)
            {
                await ReorderChannelsAsync(guildId, new Dictionary<Snowflake, int>
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
            return models.Select(x => new RestGuildVoiceRegion(this, x, guildId)).ToImmutableArray();
        }

        public async Task<IReadOnlyList<RestInvite>> GetGuildInvitesAsync(Snowflake guildId, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetGuildInvitesAsync(guildId, options).ConfigureAwait(false);
            return models.Select(x => new RestInvite(this, x)).ToImmutableArray();
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

        public async Task<RestInvite> GetInviteAsync(string code, bool withCounts = true, RestRequestOptions options = null)
        {
            try
            {
                var model = await ApiClient.GetInviteAsync(code, withCounts, options).ConfigureAwait(false);
                return new RestInvite(this, model);
            }
            catch (HttpDiscordException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound || ex.JsonErrorCode == JsonErrorCode.InviteCodeIsEitherInvalidOrTaken)
            {
                return null;
            }
        }

        public async Task<RestInvite> DeleteInviteAsync(string code, RestRequestOptions options = null)
        {
            var model = await ApiClient.DeleteInviteAsync(code, options).ConfigureAwait(false);
            return new RestInvite(this, model);
        }
    }
}
