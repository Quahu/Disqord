using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;
using Qommon;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<GuildJsonModel> CreateGuildAsync(this IRestApiClient client,
        CreateGuildJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.CreateGuild);
        return client.ExecuteAsync<GuildJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<GuildJsonModel> FetchGuildAsync(this IRestApiClient client,
        Snowflake guildId, bool? withCounts = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = withCounts != null
            ? new[] { new KeyValuePair<string, object>("with_counts", withCounts.Value) }
            : null;

        var route = Format(Route.Guild.GetGuild, queryParameters, guildId);
        return client.ExecuteAsync<GuildJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<GuildPreviewJsonModel> FetchGuildPreviewAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.GetGuildPreview, guildId);
        return client.ExecuteAsync<GuildPreviewJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<GuildJsonModel> ModifyGuildAsync(this IRestApiClient client,
        Snowflake guildId, ModifyGuildJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.ModifyGuild, guildId);
        return client.ExecuteAsync<GuildJsonModel>(route, content, options, cancellationToken);
    }

    public static Task DeleteGuildAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.DeleteGuild, guildId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }

    public static Task<ChannelJsonModel[]> FetchGuildChannelsAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.GetChannels, guildId);
        return client.ExecuteAsync<ChannelJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<ChannelJsonModel> CreateGuildChannelAsync(this IRestApiClient client,
        Snowflake guildId, CreateGuildChannelJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.CreateChannel, guildId);
        return client.ExecuteAsync<ChannelJsonModel>(route, content, options, cancellationToken);
    }

    public static Task ReorderGuildChannelsAsync(this IRestApiClient client,
        Snowflake guildId, JsonObjectRestRequestContent<ReorderJsonRestRequestContent[]> content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.ReorderChannels, guildId);
        return client.ExecuteAsync(route, content, options, cancellationToken);
    }

    public static Task<ThreadListJsonModel> FetchActiveThreadsAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.ListActiveThreads, guildId);
        return client.ExecuteAsync<ThreadListJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<MemberJsonModel> FetchMemberAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake memberId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.GetMember, guildId, memberId);
        return client.ExecuteAsync<MemberJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<MemberJsonModel[]> FetchMembersAsync(this IRestApiClient client,
        Snowflake guildId, int limit = Discord.Limits.Rest.FetchMembersPageSize, Snowflake? startFromId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsBetweenOrEqualTo(limit, 0, Discord.Limits.Rest.FetchMembersPageSize);

        var queryParameters = new Dictionary<string, object>(startFromId != null ? 2 : 1)
        {
            ["limit"] = limit
        };

        if (startFromId != null)
            queryParameters["after"] = startFromId;

        var route = Format(Route.Guild.GetMembers, queryParameters, guildId);
        return client.ExecuteAsync<MemberJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<MemberJsonModel[]> SearchMembersAsync(this IRestApiClient client,
        Snowflake guildId, string query, int limit = Discord.Limits.Rest.FetchMembersPageSize,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsBetweenOrEqualTo(limit, 0, Discord.Limits.Rest.FetchMembersPageSize);

        var queryParameters = new Dictionary<string, object>(2)
        {
            ["query"] = query,
            ["limit"] = limit
        };

        var route = Format(Route.Guild.SearchMembers, queryParameters, guildId);
        return client.ExecuteAsync<MemberJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<MemberJsonModel> AddMemberAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake userId, AddMemberJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.AddMember, guildId, userId);
        return client.ExecuteAsync<MemberJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<MemberJsonModel> ModifyMemberAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake memberId, ModifyMemberJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.ModifyMember, guildId, memberId);
        return client.ExecuteAsync<MemberJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<MemberJsonModel> ModifyCurrentMemberAsync(this IRestApiClient client,
        Snowflake guildId, ModifyCurrentMemberJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.ModifyCurrentMember, guildId);
        return client.ExecuteAsync<MemberJsonModel>(route, content, options, cancellationToken);
    }

    public static Task SetOwnNickAsync(this IRestApiClient client,
        Snowflake guildId, SetOwnNickJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.SetOwnNick, guildId);
        return client.ExecuteAsync(route, content, options, cancellationToken);
    }

    public static Task GrantRoleAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake memberId, Snowflake roleId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.GrantRole, guildId, memberId, roleId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }

    public static Task RevokeRoleAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake memberId, Snowflake roleId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.RevokeRole, guildId, memberId, roleId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }

    public static Task KickMemberAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake memberId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.KickMember, guildId, memberId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }

    public static Task<BanJsonModel[]> FetchBansAsync(this IRestApiClient client,
        Snowflake guildId,
        int limit = Discord.Limits.Rest.FetchBansPageSize, FetchDirection direction = FetchDirection.After, Snowflake? startFromId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsBetweenOrEqualTo(limit, 0, Discord.Limits.Rest.FetchBansPageSize);

        var queryParameters = new Dictionary<string, object>(startFromId != null ? 2 : 1)
        {
            ["limit"] = limit
        };

        switch (direction)
        {
            case FetchDirection.Before:
            {
                if (startFromId != null)
                    queryParameters["before"] = startFromId;

                break;
            }
            case FetchDirection.After:
            {
                if (startFromId != null)
                    queryParameters["after"] = startFromId;

                break;
            }
            default:
            {
                throw new ArgumentOutOfRangeException(nameof(direction), direction, "Invalid ban fetch direction.");
            }
        }

        var route = Format(Route.Guild.GetBans, queryParameters, guildId);
        return client.ExecuteAsync<BanJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<BanJsonModel> FetchBanAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake userId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.GetBan, guildId, userId);
        return client.ExecuteAsync<BanJsonModel>(route, null, options, cancellationToken);
    }

    public static Task CreateBanAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake userId, CreateBanJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.CreateBan, guildId, userId);
        return client.ExecuteAsync(route, content, options, cancellationToken);
    }

    public static Task DeleteBanAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake userId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.DeleteBan, guildId, userId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }

    public static Task<RoleJsonModel[]> FetchRolesAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.GetRoles, guildId);
        return client.ExecuteAsync<RoleJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<RoleJsonModel> CreateRoleAsync(this IRestApiClient client,
        Snowflake guildId, CreateRoleJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.CreateRole, guildId);
        return client.ExecuteAsync<RoleJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<RoleJsonModel[]> ReorderRolesAsync(this IRestApiClient client,
        Snowflake guildId, JsonObjectRestRequestContent<ReorderJsonRestRequestContent[]> content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.ReorderRoles, guildId);
        return client.ExecuteAsync<RoleJsonModel[]>(route, content, options, cancellationToken);
    }

    public static Task<RoleJsonModel> ModifyRoleAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake roleId, ModifyRoleJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.ModifyRole, guildId, roleId);
        return client.ExecuteAsync<RoleJsonModel>(route, content, options, cancellationToken);
    }

    public static Task DeleteRoleAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake roleId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.DeleteRole, guildId, roleId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }

    public static Task<PruneJsonModel> FetchPruneGuildCountAsync(this IRestApiClient client,
        Snowflake guildId, int days = 7, Snowflake[]? roleIds = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = new Dictionary<string, object>(roleIds != null ? 2 : 1)
        {
            ["days"] = days
        };

        if (roleIds != null)
            queryParameters["include_roles"] = roleIds;

        var route = Format(Route.Guild.GetPruneCount, queryParameters, guildId);
        return client.ExecuteAsync<PruneJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<PruneJsonModel> PruneGuildAsync(this IRestApiClient client,
        Snowflake guildId, int days = 7, bool computePruneCount = false, Snowflake[]? roleIds = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = new Dictionary<string, object>(roleIds != null ? 3 : 2)
        {
            ["days"] = days,
            ["compute_prune_count"] = computePruneCount
        };

        if (roleIds != null)
            queryParameters["include_roles"] = roleIds;

        var route = Format(Route.Guild.BeginPrune, queryParameters, guildId);
        return client.ExecuteAsync<PruneJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<VoiceRegionJsonModel[]> FetchGuildVoiceRegionsAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.GetVoiceRegions, guildId);
        return client.ExecuteAsync<VoiceRegionJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<InviteJsonModel[]> FetchGuildInvitesAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.GetInvites, guildId);
        return client.ExecuteAsync<InviteJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<IntegrationJsonModel[]> FetchIntegrationsAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.GetIntegrations, guildId);
        return client.ExecuteAsync<IntegrationJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task CreateIntegrationAsync(this IRestApiClient client,
        Snowflake guildId, CreateGuildIntegrationJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.CreateIntegration, guildId);
        return client.ExecuteAsync(route, content, options, cancellationToken);
    }

    public static Task ModifyIntegrationAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake integrationId, ModifyGuildIntegrationJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.ModifyIntegration, guildId, integrationId);
        return client.ExecuteAsync(route, content, options, cancellationToken);
    }

    public static Task DeleteIntegrationAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake integrationId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.DeleteIntegration, guildId, integrationId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }

    public static Task SyncIntegrationAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake integrationId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.SyncIntegration, guildId, integrationId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }

    public static Task<WelcomeScreenJsonModel> FetchGuildWelcomeScreenAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.GetWelcomeScreen, guildId);
        return client.ExecuteAsync<WelcomeScreenJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<WelcomeScreenJsonModel> ModifyGuildWelcomeScreenAsync(this IRestApiClient client,
        Snowflake guildId, ModifyWelcomeScreenJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.ModifyWelcomeScreen, guildId);
        return client.ExecuteAsync<WelcomeScreenJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<GuildWidgetJsonModel> FetchGuildWidgetAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.GetWidgetSettings, guildId);
        return client.ExecuteAsync<GuildWidgetJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<GuildWidgetJsonModel> ModifyGuildWidgetAsync(this IRestApiClient client,
        Snowflake guildId, ModifyGuildWidgetSettingsJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.ModifyWidget, guildId);
        return client.ExecuteAsync<GuildWidgetJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<InviteJsonModel> FetchGuildVanityInviteAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.GetVanityUrl, guildId);
        return client.ExecuteAsync<InviteJsonModel>(route, null, options, cancellationToken);
    }

    public static Task ModifyCurrentMemberVoiceStateAsync(this IRestApiClient client,
        Snowflake guildId, ModifyCurrentMemberVoiceStateJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.ModifyCurrentMemberVoiceState, guildId);
        return client.ExecuteAsync(route, content, options, cancellationToken);
    }

    public static Task ModifyMemberVoiceStateAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake memberId, ModifyMemberVoiceStateJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Guild.ModifyMemberVoiceState, guildId, memberId);
        return client.ExecuteAsync(route, content, options, cancellationToken);
    }
}
