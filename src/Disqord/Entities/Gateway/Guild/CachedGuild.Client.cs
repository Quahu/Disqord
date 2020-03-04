using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Disqord.Rest;
using Disqord.Rest.AuditLogs;

namespace Disqord
{
    public sealed partial class CachedGuild : CachedSnowflakeEntity, IGuild
    {
        // IDeletable
        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteGuildAsync(Id, options);

        // IGuild
        public Task<IReadOnlyList<RestWebhook>> GetWebhooksAsync(RestRequestOptions options = null)
            => Client.GetGuildWebhooksAsync(Id, options);

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
            var model = await Client.RestClient.InternalModifyGuildAsync(Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public Task<IReadOnlyList<RestGuildChannel>> GetChannelsAsync(RestRequestOptions options = null)
            => Client.GetChannelsAsync(Id, options);

        public Task<RestTextChannel> CreateTextChannelAsync(string name, Action<CreateTextChannelProperties> action = null, RestRequestOptions options = null)
            => Client.CreateTextChannelAsync(Id, name, action, options);

        public Task<RestVoiceChannel> CreateVoiceChannelAsync(string name, Action<CreateVoiceChannelProperties> action = null, RestRequestOptions options = null)
            => Client.CreateVoiceChannelAsync(Id, name, action, options);

        public Task<RestCategoryChannel> CreateCategoryChannelAsync(string name, Action<CreateCategoryChannelProperties> action = null, RestRequestOptions options = null)
            => Client.CreateCategoryChannelAsync(Id, name, action, options);

        public Task ReorderChannelsAsync(IReadOnlyDictionary<Snowflake, int> positions, RestRequestOptions options = null)
            => Client.ReorderChannelsAsync(Id, positions, options);

        public Task<RestMember> GetMemberAsync(Snowflake memberId, RestRequestOptions options = null)
            => Client.GetMemberAsync(Id, memberId, options);

        public RestRequestEnumerable<RestMember> GetMembersEnumerable(int limit, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetMembersEnumerable(Id, limit, startFromId, options);

        public Task<IReadOnlyList<RestMember>> GetMembersAsync(int limit = 1000, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetMembersAsync(Id, limit, startFromId, options);

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

        public Task<IReadOnlyList<RestBan>> GetBansAsync(RestRequestOptions options = null)
            => Client.GetBansAsync(Id, options);

        public Task<RestBan> GetBanAsync(Snowflake userId, RestRequestOptions options = null)
            => Client.GetBanAsync(Id, userId, options);

        public Task BanMemberAsync(Snowflake memberId, string reason = null, int? deleteMessageDays = null, RestRequestOptions options = null)
            => Client.BanMemberAsync(Id, memberId, reason, deleteMessageDays, options);

        public Task UnbanMemberAsync(Snowflake userId, RestRequestOptions options = null)
            => Client.UnbanMemberAsync(Id, userId, options);

        public Task<IReadOnlyList<RestRole>> GetRolesAsync(RestRequestOptions options = null)
            => Client.GetRolesAsync(Id, options);

        public Task<RestRole> CreateRoleAsync(Action<CreateRoleProperties> action = null, RestRequestOptions options = null)
            => Client.CreateRoleAsync(Id, action, options);

        public Task<IReadOnlyList<RestRole>> ReorderRolesAsync(IReadOnlyDictionary<Snowflake, int> positions, RestRequestOptions options = null)
            => Client.ReorderRolesAsync(Id, positions, options);

        public Task<RestRole> ModifyRoleAsync(Snowflake roleId, Action<ModifyRoleProperties> action, RestRequestOptions options = null)
            => Client.ModifyRoleAsync(Id, roleId, action, options);

        public Task DeleteRoleAsync(Snowflake roleId, RestRequestOptions options = null)
            => Client.DeleteRoleAsync(Id, roleId, options);

        public Task<int> GetPruneCountAsync(int days, RestRequestOptions options = null)
            => Client.GetPruneCountAsync(Id, days, options);

        public Task<int?> PruneAsync(int days, bool computePruneCount = true, RestRequestOptions options = null)
            => Client.PruneAsync(Id, days, computePruneCount, options);

        public Task<IReadOnlyList<RestGuildVoiceRegion>> GetVoiceRegionsAsync(RestRequestOptions options = null)
            => Client.GetVoiceRegionsAsync(Id, options);

        public Task<IReadOnlyList<RestInvite>> GetInvitesAsync(RestRequestOptions options = null)
            => Client.GetGuildInvitesAsync(Id, options);

        public Task<RestWidget> GetWidgetAsync(RestRequestOptions options = null)
            => Client.GetWidgetAsync(Id, options);

        public Task<RestWidget> ModifyWidgetAsync(Action<ModifyWidgetProperties> action, RestRequestOptions options = null)
            => Client.ModifyWidgetAsync(Id, action, options);

        public Task<string> GetVanityInviteAsync(RestRequestOptions options = null)
            => Client.GetVanityInviteAsync(Id, options);

        public Task LeaveAsync(RestRequestOptions options = null)
            => Client.LeaveGuildAsync(Id, options);

        public Task<IReadOnlyList<RestGuildEmoji>> GetEmojisAsync(RestRequestOptions options = null)
            => Client.GetGuildEmojisAsync(Id, options);

        public Task<RestGuildEmoji> GetEmojiAsync(Snowflake emojiId, RestRequestOptions options = null)
            => Client.GetGuildEmojiAsync(Id, emojiId, options);

        public Task<RestGuildEmoji> CreateEmojiAsync(Stream image, string name, IEnumerable<Snowflake> roleIds = null, RestRequestOptions options = null)
            => Client.CreateGuildEmojiAsync(Id, image, name, roleIds, options);

        public Task<RestGuildEmoji> ModifyEmojiAsync(Snowflake emojiId, Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null)
            => Client.ModifyGuildEmojiAsync(Id, emojiId, action, options);

        public Task DeleteEmojiAsync(Snowflake emojiId, RestRequestOptions options = null)
            => Client.DeleteGuildEmojiAsync(Id, emojiId, options);

        public Task<RestPreview> GetPreviewAsync(RestRequestOptions options = null)
            => Client.GetPreviewAsync(Id, options);
    }
}
