using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Disqord.Rest.AuditLogs;

namespace Disqord.Rest
{
    public sealed partial class RestGuild : RestSnowflakeEntity, IGuild
    {
        // IDeletable
        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteGuildAsync(Id, options);

        // IGuild
        public async Task<IReadOnlyList<RestWebhook>> GetWebhooksAsync(RestRequestOptions options = null)
        {
            var webhooks = await Client.GetGuildWebhooksAsync(Id, options).ConfigureAwait(false);
            for (var i = 0; i < webhooks.Count; i++)
                webhooks[i].Guild.Value = this;

            return webhooks;
        }

        public RestRequestEnumerable<RestAuditLog> GetAuditLogsEnumerable(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetAuditLogsEnumerable(Id, limit, userId, startFromId, options);

        public RestRequestEnumerable<T> GetAuditLogsEnumerable<T>(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog
            => Client.GetAuditLogsEnumerable<T>(Id, limit, userId, startFromId, options);

        public Task<IReadOnlyList<RestAuditLog>> GetAuditLogsAsync(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetAuditLogsAsync(Id, limit, userId, startFromId, options);

        public Task<IReadOnlyList<T>> GetAuditLogsAsync<T>(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog
            => Client.GetAuditLogsAsync<T>(Id, limit, userId, startFromId, options);

        public async Task ModifyAsync(Action<ModifyGuildProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyGuildAsync(Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public async Task<IReadOnlyList<RestGuildChannel>> GetChannelsAsync(RestRequestOptions options = null)
        {
            var channels = await Client.GetChannelsAsync(Id, options).ConfigureAwait(false);
            for (var i = 0; i < channels.Count; i++)
                channels[i].Guild.Value = this;

            return channels;
        }

        public Task<RestTextChannel> CreateTextChannelAsync(string name, Action<CreateTextChannelProperties> action = null, RestRequestOptions options = null)
            => Client.CreateTextChannelAsync(Id, name, action, options);

        public Task<RestVoiceChannel> CreateVoiceChannelAsync(string name, Action<CreateVoiceChannelProperties> action = null, RestRequestOptions options = null)
            => Client.CreateVoiceChannelAsync(Id, name, action, options);

        public Task<RestCategoryChannel> CreateCategoryChannelAsync(string name, Action<CreateCategoryChannelProperties> action = null, RestRequestOptions options = null)
            => Client.CreateCategoryChannelAsync(Id, name, action, options);

        public Task ReorderChannelsAsync(IReadOnlyDictionary<Snowflake, int> positions, RestRequestOptions options = null)
            => Client.ReorderChannelsAsync(Id, positions, options);

        public async Task<RestMember> GetMemberAsync(Snowflake memberId, RestRequestOptions options = null)
        {
            var member = await Client.GetMemberAsync(Id, memberId, options).ConfigureAwait(false);
            member.Guild.Value = this;
            return member;
        }

        public RestRequestEnumerable<RestMember> GetMembersEnumerable(int limit, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetMembersEnumerable(Id, limit, startFromId, options);

        public async Task<IReadOnlyList<RestMember>> GetMembersAsync(int limit = 1000, Snowflake? startFromId = null, RestRequestOptions options = null)
        {
            var members = await Client.GetMembersAsync(Id, limit, startFromId, options).ConfigureAwait(false);
            for (var i = 0; i < members.Count; i++)
                members[i].Guild.Value = this;

            return members;
        }

        public Task ModifyMemberAsync(Snowflake memberId, Action<ModifyMemberProperties> action, RestRequestOptions options = null)
            => Client.ModifyMemberAsync(Id, memberId, action, options);

        public Task ModifyOwnNickAsync(string nick, RestRequestOptions options = null)
            => Client.ModifyOwnNickAsync(Id, nick, options);

        public Task GrantRoleAsync(Snowflake memberId, Snowflake roleId, RestRequestOptions options = null)
            => Client.GrantRoleAsync(Id, memberId, roleId, options);

        public Task RevokeRoleAsync(Snowflake memberId, Snowflake roleId, RestRequestOptions options = null)
            => Client.RevokeRoleAsync(Id, memberId, roleId, options);

        public Task KickMemberAsync(Snowflake memberId, RestRequestOptions options = null)
            => Client.KickMemberAsync(Id, memberId, options);

        public async Task<IReadOnlyList<RestBan>> GetBansAsync(RestRequestOptions options = null)
        {
            var bans = await Client.GetBansAsync(Id, options).ConfigureAwait(false);
            for (var i = 0; i < bans.Count; i++)
                bans[i].Guild.Value = this;

            return bans;
        }

        public async Task<RestBan> GetBanAsync(Snowflake userId, RestRequestOptions options = null)
        {
            var ban = await Client.GetBanAsync(Id, userId, options).ConfigureAwait(false);
            ban.Guild.Value = this;
            return ban;
        }

        public Task BanMemberAsync(Snowflake memberId, string reason = null, int? deleteMessageDays = null, RestRequestOptions options = null)
            => Client.BanMemberAsync(Id, memberId, reason, deleteMessageDays, options);

        public Task UnbanMemberAsync(Snowflake userId, RestRequestOptions options = null)
            => Client.UnbanMemberAsync(Id, userId, options);

        public async Task<IReadOnlyList<RestRole>> GetRolesAsync(RestRequestOptions options = null)
        {
            var roles = await Client.GetRolesAsync(Id, options).ConfigureAwait(false);
            for (var i = 0; i < roles.Count; i++)
                roles[i].Guild.Value = this;

            return roles;
        }

        public async Task<RestRole> CreateRoleAsync(Action<CreateRoleProperties> action = null, RestRequestOptions options = null)
        {
            var role = await Client.CreateRoleAsync(Id, action, options).ConfigureAwait(false);
            role.Guild.Value = this;
            return role;
        }

        public async Task<IReadOnlyList<RestRole>> ReorderRolesAsync(IReadOnlyDictionary<Snowflake, int> positions, RestRequestOptions options = null)
        {
            var roles = await Client.ReorderRolesAsync(Id, positions, options).ConfigureAwait(false);
            for (var i = 0; i < roles.Count; i++)
                roles[i].Guild.Value = this;

            return roles;
        }

        public async Task<RestRole> ModifyRoleAsync(Snowflake roleId, Action<ModifyRoleProperties> action, RestRequestOptions options = null)
        {
            var role = await Client.ModifyRoleAsync(Id, roleId, action, options).ConfigureAwait(false);
            role.Guild.Value = this;
            return role;
        }

        public Task DeleteRoleAsync(Snowflake roleId, RestRequestOptions options = null)
            => Client.DeleteRoleAsync(Id, roleId, options);

        public Task<int> GetPruneCountAsync(int days, RestRequestOptions options = null)
            => Client.GetPruneCountAsync(Id, days, options);

        public Task<int?> PruneAsync(int days, bool computePruneCount = true, RestRequestOptions options = null)
            => Client.PruneAsync(Id, days, computePruneCount, options);

        public async Task<IReadOnlyList<RestGuildVoiceRegion>> GetVoiceRegionsAsync(RestRequestOptions options = null)
        {
            var regions = await Client.GetVoiceRegionsAsync(Id, options).ConfigureAwait(false);
            for (var i = 0; i < regions.Count; i++)
                regions[i].Guild.Value = this;

            return regions;
        }

        public Task<IReadOnlyList<RestInvite>> GetInvitesAsync(RestRequestOptions options = null)
            => Client.GetGuildInvitesAsync(Id, options);

        public async Task<RestWidget> GetWidgetAsync(RestRequestOptions options = null)
        {
            var widget = await Client.GetWidgetAsync(Id, options).ConfigureAwait(false);
            widget.Guild.Value = this;
            return widget;
        }

        public async Task<RestWidget> ModifyWidgetAsync(Action<ModifyWidgetProperties> action, RestRequestOptions options = null)
        {
            var widget = await Client.ModifyWidgetAsync(Id, action, options).ConfigureAwait(false);
            widget.Guild.Value = this;
            return widget;
        }

        public Task<string> GetVanityInviteAsync(RestRequestOptions options = null)
            => Client.GetVanityInviteAsync(Id, options);

        public Task LeaveAsync(RestRequestOptions options = null)
            => Client.LeaveGuildAsync(Id, options);

        public async Task<IReadOnlyList<RestGuildEmoji>> GetEmojisAsync(RestRequestOptions options = null)
        {
            var emojis = await Client.GetGuildEmojisAsync(Id, options).ConfigureAwait(false);
            for (var i = 0; i < emojis.Count; i++)
                emojis[i].Guild.Value = this;

            return emojis;
        }

        public async Task<RestGuildEmoji> GetEmojiAsync(Snowflake emojiId, RestRequestOptions options = null)
        {
            var emoji = await Client.GetGuildEmojiAsync(Id, emojiId, options).ConfigureAwait(false);
            emoji.Guild.Value = this;
            return emoji;
        }

        public async Task<RestGuildEmoji> CreateEmojiAsync(Stream image, string name, IEnumerable<Snowflake> roleIds = null, RestRequestOptions options = null)
        {
            var emoji = await Client.CreateGuildEmojiAsync(Id, image, name, roleIds, options).ConfigureAwait(false);
            emoji.Guild.Value = this;
            return emoji;
        }

        public async Task<RestGuildEmoji> ModifyEmojiAsync(Snowflake emojiId, Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null)
        {
            var emoji = await Client.ModifyGuildEmojiAsync(Id, emojiId, action, options).ConfigureAwait(false);
            emoji.Guild.Value = this;
            return emoji;
        }

        public Task DeleteEmojiAsync(Snowflake emojiId, RestRequestOptions options = null)
            => Client.DeleteGuildEmojiAsync(Id, emojiId, options);

        public async Task<RestPreview> GetPreviewAsync(RestRequestOptions options = null)
        {
            var preview = await Client.GetPreviewAsync(Id, options).ConfigureAwait(false);
            preview.Guild.Value = this;
            return preview;
        }
    }
}
