using Disqord.Rest;
using Disqord.Rest.AuditLogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord
{
    public partial interface IGuild : ISnowflakeEntity, IDeletable
    {
        Task<IReadOnlyList<RestWebhook>> GetWebhooksAsync(RestRequestOptions options = null);

        RestRequestEnumerator<RestAuditLog> GetAuditLogsEnumerator(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null);

        RestRequestEnumerator<T> GetAuditLogsEnumerator<T>(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null) where T : RestAuditLog;

        Task<IReadOnlyList<RestAuditLog>> GetAuditLogsAsync(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null);

        Task<IReadOnlyList<T>> GetAuditLogsAsync<T>(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog;

        Task ModifyAsync(Action<ModifyGuildProperties> action, RestRequestOptions options = null);

        Task ReorderChannelsAsync(IReadOnlyDictionary<Snowflake, int> positions, RestRequestOptions options = null);

        Task<RestMember> GetMemberAsync(Snowflake memberId, RestRequestOptions options = null);

        RestRequestEnumerator<RestMember> GetMembersEnumerator(int limit, Snowflake? startFromId = null);

        Task<IReadOnlyList<RestMember>> GetMembersAsync(int limit = 1000, Snowflake? startFromId = null, RestRequestOptions options = null);

        Task ModifyMemberAsync(Snowflake memberId, Action<ModifyMemberProperties> action, RestRequestOptions options = null);

        Task ModifyOwnNickAsync(string nick, RestRequestOptions options = null);

        Task GrantRoleAsync(Snowflake memberId, Snowflake roleId, RestRequestOptions options = null);

        Task RevokeRoleAsync(Snowflake memberId, Snowflake roleId, RestRequestOptions options = null);

        Task KickMemberAsync(Snowflake memberId, RestRequestOptions options = null);

        Task<IReadOnlyList<RestBan>> GetBansAsync(RestRequestOptions options = null);

        Task<RestBan> GetBanAsync(Snowflake userId, RestRequestOptions options = null);

        Task BanMemberAsync(Snowflake memberId, int? deleteMessageDays = null, string reason = null, RestRequestOptions options = null);
        
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
        
        Task<RestGuildEmoji> CreateEmojiAsync(string name, LocalAttachment image, IEnumerable<Snowflake> roleIds = null, RestRequestOptions options = null);
        
        Task<RestGuildEmoji> ModifyEmojiAsync(Snowflake emojiId, Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null);
        
        Task DeleteEmojiAsync(Snowflake emojiId, RestRequestOptions options = null);
    }
}