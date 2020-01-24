using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Disqord.Rest;
using Disqord.Rest.AuditLogs;

namespace Disqord
{
    public partial interface IGuild : ISnowflakeEntity, IDeletable
    {
        Task<IReadOnlyList<RestWebhook>> GetWebhooksAsync(RestRequestOptions options = null);

        RestRequestEnumerable<RestAuditLog> GetAuditLogsEnumerable(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null);

        RestRequestEnumerable<T> GetAuditLogsEnumerable<T>(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog;

        Task<IReadOnlyList<RestAuditLog>> GetAuditLogsAsync(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null);

        Task<IReadOnlyList<T>> GetAuditLogsAsync<T>(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog;

        Task ModifyAsync(Action<ModifyGuildProperties> action, RestRequestOptions options = null);

        Task<IReadOnlyList<RestGuildChannel>> GetChannelsAsync(RestRequestOptions options = null);

        Task<RestTextChannel> CreateTextChannelAsync(string name, Action<CreateTextChannelProperties> action = null, RestRequestOptions options = null);

        Task<RestVoiceChannel> CreateVoiceChannelAsync(string name, Action<CreateVoiceChannelProperties> action = null, RestRequestOptions options = null);

        Task<RestCategoryChannel> CreateCategoryChannelAsync(string name, Action<CreateCategoryChannelProperties> action = null, RestRequestOptions options = null);

        Task ReorderChannelsAsync(IReadOnlyDictionary<Snowflake, int> positions, RestRequestOptions options = null);

        Task<RestMember> GetMemberAsync(Snowflake memberId, RestRequestOptions options = null);

        RestRequestEnumerable<RestMember> GetMembersEnumerable(int limit, Snowflake? startFromId = null, RestRequestOptions options = null);

        Task<IReadOnlyList<RestMember>> GetMembersAsync(int limit = 1000, Snowflake? startFromId = null, RestRequestOptions options = null);

        Task ModifyMemberAsync(Snowflake memberId, Action<ModifyMemberProperties> action, RestRequestOptions options = null);

        Task ModifyOwnNickAsync(string nick, RestRequestOptions options = null);

        Task GrantRoleAsync(Snowflake memberId, Snowflake roleId, RestRequestOptions options = null);

        Task RevokeRoleAsync(Snowflake memberId, Snowflake roleId, RestRequestOptions options = null);

        Task KickMemberAsync(Snowflake memberId, RestRequestOptions options = null);

        Task<IReadOnlyList<RestBan>> GetBansAsync(RestRequestOptions options = null);

        Task<RestBan> GetBanAsync(Snowflake userId, RestRequestOptions options = null);

        Task BanMemberAsync(Snowflake memberId, string reason = null, int? deleteMessageDays = null, RestRequestOptions options = null);

        Task UnbanMemberAsync(Snowflake userId, RestRequestOptions options = null);

        Task<IReadOnlyList<RestRole>> GetRolesAsync(RestRequestOptions options = null);

        Task<RestRole> CreateRoleAsync(Action<CreateRoleProperties> action = null, RestRequestOptions options = null);

        Task<IReadOnlyList<RestRole>> ReorderRolesAsync(IReadOnlyDictionary<Snowflake, int> positions, RestRequestOptions options = null);

        Task<RestRole> ModifyRoleAsync(Snowflake roleId, Action<ModifyRoleProperties> action, RestRequestOptions options = null);

        Task DeleteRoleAsync(Snowflake roleId, RestRequestOptions options = null);

        Task<int> GetPruneCountAsync(int days, RestRequestOptions options = null);

        Task<int?> PruneAsync(int days, bool computePruneCount = true, RestRequestOptions options = null);

        Task<IReadOnlyList<RestGuildVoiceRegion>> GetVoiceRegionsAsync(RestRequestOptions options = null);

        Task<IReadOnlyList<RestInvite>> GetInvitesAsync(RestRequestOptions options = null);

        Task<RestWidget> GetWidgetAsync(RestRequestOptions options = null);

        Task<RestWidget> ModifyWidgetAsync(Action<ModifyWidgetProperties> action, RestRequestOptions options = null);

        Task<string> GetVanityInviteAsync(RestRequestOptions options = null);

        Task LeaveAsync(RestRequestOptions options = null);

        Task<IReadOnlyList<RestGuildEmoji>> GetEmojisAsync(RestRequestOptions options = null);

        Task<RestGuildEmoji> GetEmojiAsync(Snowflake emojiId, RestRequestOptions options = null);

        Task<RestGuildEmoji> CreateEmojiAsync(Stream image, string name, IEnumerable<Snowflake> roleIds = null, RestRequestOptions options = null);

        Task<RestGuildEmoji> ModifyEmojiAsync(Snowflake emojiId, Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null);

        Task DeleteEmojiAsync(Snowflake emojiId, RestRequestOptions options = null);
    }
}