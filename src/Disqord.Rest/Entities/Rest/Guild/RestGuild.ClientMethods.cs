using System;
using System.Collections.Generic;
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
                webhooks[i].Guild.SetValue(this);

            return webhooks;
        }

        public RestRequestEnumerator<RestAuditLog> GetAuditLogsEnumerator(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null)
            => Client.GetAuditLogsEnumerator(Id, limit, userId, startFromId);

        public RestRequestEnumerator<T> GetAuditLogsEnumerator<T>(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null) where T : RestAuditLog
            => Client.GetAuditLogsEnumerator<T>(Id, limit, userId, startFromId);

        public Task<IReadOnlyList<RestAuditLog>> GetAuditLogsAsync(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetAuditLogsAsync(Id, limit, userId, startFromId, options);

        public Task<IReadOnlyList<T>> GetAuditLogsAsync<T>(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog
            => Client.GetAuditLogsAsync<T>(Id, limit, userId, startFromId, options);

        public async Task ModifyAsync(Action<ModifyGuildProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyGuildAsync(Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public Task ReorderChannelsAsync(IReadOnlyDictionary<Snowflake, int> positions, RestRequestOptions options = null)
            => Client.ReorderChannelsAsync(Id, positions, options);

        public async Task<RestMember> GetMemberAsync(Snowflake memberId, RestRequestOptions options = null)
        {
            var member = await Client.GetMemberAsync(Id, memberId, options).ConfigureAwait(false);
            member.Guild.SetValue(this);
            return member;
        }

        public RestRequestEnumerator<RestMember> GetMembersEnumerator(int limit, Snowflake? startFromId = null)
            => Client.GetMembersEnumerator(Id, limit, startFromId);

        public async Task<IReadOnlyList<RestMember>> GetMembersAsync(int limit = 1000, Snowflake? startFromId = null, RestRequestOptions options = null)
        {
            var members = await Client.GetMembersAsync(Id, limit, startFromId, options).ConfigureAwait(false);
            for (var i = 0; i < members.Count; i++)
                members[i].Guild.SetValue(this);

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
                bans[i].Guild.SetValue(this);

            return bans;
        }

        public async Task<RestBan> GetBanAsync(Snowflake userId, RestRequestOptions options = null)
        {
            var ban = await Client.GetBanAsync(Id, userId, options).ConfigureAwait(false);
            ban.Guild.SetValue(this);
            return ban;
        }

        public Task BanMemberAsync(Snowflake memberId, int? deleteMessageDays = null, string reason = null, RestRequestOptions options = null)
            => Client.BanMemberAsync(Id, memberId, deleteMessageDays, reason, options);

        public Task UnbanMemberAsync(Snowflake userId, RestRequestOptions options = null)
            => Client.UnbanMemberAsync(Id, userId, options);

        public async Task<IReadOnlyList<RestRole>> GetRolesAsync(RestRequestOptions options = null)
        {
            var roles = await Client.GetRolesAsync(Id, options).ConfigureAwait(false);
            for (var i = 0; i < roles.Count; i++)
                roles[i].Guild.SetValue(this);

            return roles;
        }

        public async Task<RestRole> CreateRoleAsync(Action<CreateRoleProperties> action = null, RestRequestOptions options = null)
        {
            var role = await Client.CreateRoleAsync(Id, action, options);
            role.Guild.SetValue(this);
            return role;
        }

        public async Task<IReadOnlyList<RestRole>> ReorderRolesAsync(IReadOnlyDictionary<Snowflake, int> positions, RestRequestOptions options = null)
        {
            var roles = await Client.ReorderRolesAsync(Id, positions, options).ConfigureAwait(false);
            for (var i = 0; i < roles.Count; i++)
                roles[i].Guild.SetValue(this);

            return roles;
        }

        public async Task<RestRole> ModifyRoleAsync(Snowflake roleId, Action<ModifyRoleProperties> action, RestRequestOptions options = null)
        {
            var role = await Client.ModifyRoleAsync(Id, roleId, action, options).ConfigureAwait(false);
            role.Guild.SetValue(this);
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
                regions[i].Guild.SetValue(this);

            return regions;
        }

        public Task<IReadOnlyList<RestInvite>> GetInvitesAsync(RestRequestOptions options = null)
            => Client.GetGuildInvitesAsync(Id, options);

        public async Task<RestWidget> GetWidgetAsync(RestRequestOptions options = null)
        {
            var widget = await Client.GetWidgetAsync(Id, options).ConfigureAwait(false);
            widget.Guild.SetValue(this);
            return widget;
        }

        public async Task<RestWidget> ModifyWidgetAsync(Action<ModifyWidgetProperties> action, RestRequestOptions options = null)
        {
            var widget = await Client.ModifyWidgetAsync(Id, action, options).ConfigureAwait(false);
            widget.Guild.SetValue(this);
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
                emojis[i].Guild.SetValue(this);
            return emojis;
        }

        public async Task<RestGuildEmoji> GetEmojiAsync(Snowflake emojiId, RestRequestOptions options = null)
        {
            var emoji = await Client.GetGuildEmojiAsync(Id, emojiId, options).ConfigureAwait(false);
            emoji.Guild.SetValue(this);
            return emoji;
        }

        public async Task<RestGuildEmoji> CreateEmojiAsync(string name, LocalAttachment image, IEnumerable<Snowflake> roleIds = null, RestRequestOptions options = null)
        {
            var emoji = await Client.CreateGuildEmojiAsync(Id, name, image, roleIds, options).ConfigureAwait(false);
            emoji.Guild.SetValue(this);
            return emoji;
        }

        public async Task<RestGuildEmoji> ModifyEmojiAsync(Snowflake emojiId, Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null)
        {
            var emoji = await Client.ModifyGuildEmojiAsync(Id, emojiId, action, options).ConfigureAwait(false);
            emoji.Guild.SetValue(this);
            return emoji;
        }

        public Task DeleteEmojiAsync(Snowflake emojiId, RestRequestOptions options = null)
            => Client.DeleteGuildEmojiAsync(Id, emojiId, options);
    }
}
