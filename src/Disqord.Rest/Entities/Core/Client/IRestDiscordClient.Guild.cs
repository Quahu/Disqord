﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<RestGuild> CreateGuildAsync(string name, string voiceRegionId = null, LocalAttachment icon = null, VerificationLevel verificationLevel = default,
            DefaultNotificationLevel defaultNotificationLevel = default, ContentFilterLevel contentFilterLevel = default,
            RestRequestOptions options = null);

        Task<RestGuild> GetGuildAsync(Snowflake guildId, RestRequestOptions options = null);

        Task<RestGuild> ModifyGuildAsync(Snowflake guildId, Action<ModifyGuildProperties> action, RestRequestOptions options = null);

        Task DeleteGuildAsync(Snowflake guildId, RestRequestOptions options = null);

        Task<IReadOnlyList<RestGuildChannel>> GetChannelsAsync(Snowflake guildId, RestRequestOptions options = null);

        // TODO: create guild channel

        Task ReorderChannelsAsync(Snowflake guildId, IReadOnlyDictionary<Snowflake, int> positions, RestRequestOptions options = null);

        Task<RestMember> GetMemberAsync(Snowflake guildId, Snowflake memberId, RestRequestOptions options = null);

        RestRequestEnumerator<RestMember> GetMembersEnumerator(Snowflake guildId, int limit, Snowflake? startFromId = null);

        Task<IReadOnlyList<RestMember>> GetMembersAsync(Snowflake guildId, int limit = 1000, Snowflake? startFromId = null, RestRequestOptions options = null);

        // TODO: add member

        Task ModifyMemberAsync(Snowflake guildId, Snowflake memberId, Action<ModifyMemberProperties> action, RestRequestOptions options = null);

        Task ModifyOwnNickAsync(Snowflake guildId, string nick, RestRequestOptions options = null);

        Task GrantRoleAsync(Snowflake guildId, Snowflake memberId, Snowflake roleId, RestRequestOptions options = null);

        Task RevokeRoleAsync(Snowflake guildId, Snowflake memberId, Snowflake roleId, RestRequestOptions options = null);

        Task KickMemberAsync(Snowflake guildId, Snowflake memberId, RestRequestOptions options = null);

        Task<IReadOnlyList<RestBan>> GetBansAsync(Snowflake guildId, RestRequestOptions options = null);

        Task<RestBan> GetBanAsync(Snowflake guildId, Snowflake userId, RestRequestOptions options = null);

        Task BanMemberAsync(Snowflake guildId, Snowflake memberId, int? deleteMessageDays = null, string reason = null, RestRequestOptions options = null);

        Task UnbanMemberAsync(Snowflake guildId, Snowflake userId, RestRequestOptions options = null);

        Task<IReadOnlyList<RestRole>> GetRolesAsync(Snowflake guildId, RestRequestOptions options = null);

        Task<RestRole> CreateRoleAsync(Snowflake guildId, Action<CreateRoleProperties> action = null, RestRequestOptions options = null);

        Task<IReadOnlyList<RestRole>> ReorderRolesAsync(Snowflake guildId, IReadOnlyDictionary<Snowflake, int> positions, RestRequestOptions options = null);

        Task<RestRole> ModifyRoleAsync(Snowflake guildId, Snowflake roleId, Action<ModifyRoleProperties> action, RestRequestOptions options = null);

        Task DeleteRoleAsync(Snowflake guildId, Snowflake roleId, RestRequestOptions options = null);

        Task<int> GetPruneCountAsync(Snowflake guildId, int days, RestRequestOptions options = null);

        Task<int?> PruneAsync(Snowflake guildId, int days, bool computePruneCount = true, RestRequestOptions options = null);

        Task<IReadOnlyList<RestGuildVoiceRegion>> GetVoiceRegionsAsync(Snowflake guildId, RestRequestOptions options = null);

        Task<IReadOnlyList<RestInvite>> GetGuildInvitesAsync(Snowflake guildId, RestRequestOptions options = null);

        // TODO: get integrations 
        // TODO: create integration
        // TODO: modify integration
        // TODO: delete integration
        // TODO: sync integration

        Task<RestWidget> GetWidgetAsync(Snowflake guildId, RestRequestOptions options = null);

        Task<RestWidget> ModifyWidgetAsync(Snowflake guildId, Action<ModifyWidgetProperties> action, RestRequestOptions options = null);

        Task<string> GetVanityInviteAsync(Snowflake guildId, RestRequestOptions options = null);

    }
}
