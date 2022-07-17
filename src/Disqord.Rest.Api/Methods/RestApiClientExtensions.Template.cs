using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<GuildTemplateJsonModel> FetchGuildTemplateAsync(this IRestApiClient client,
        string templateCode,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Template.GetTemplate, templateCode);
        return client.ExecuteAsync<GuildTemplateJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<GuildJsonModel> CreateGuildFromTemplateAsync(this IRestApiClient client,
        string templateCode, CreateGuildFromTemplateJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Template.CreateGuild, templateCode);
        return client.ExecuteAsync<GuildJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<GuildTemplateJsonModel[]> FetchGuildTemplatesAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Template.GetTemplates, guildId);
        return client.ExecuteAsync<GuildTemplateJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<GuildTemplateJsonModel> CreateGuildTemplateAsync(this IRestApiClient client,
        Snowflake guildId, CreateGuildTemplateJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Template.CreateTemplate, guildId);
        return client.ExecuteAsync<GuildTemplateJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<GuildTemplateJsonModel> SynchronizeGuildTemplateAsync(this IRestApiClient client,
        Snowflake guildId, string templateCode,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Template.SyncTemplate, guildId, templateCode);
        return client.ExecuteAsync<GuildTemplateJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<GuildTemplateJsonModel> ModifyGuildTemplateAsync(this IRestApiClient client,
        Snowflake guildId, string templateCode, ModifyGuildTemplateJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Template.ModifyTemplate, guildId, templateCode);
        return client.ExecuteAsync<GuildTemplateJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<GuildTemplateJsonModel> DeleteGuildTemplateAsync(this IRestApiClient client,
        Snowflake guildId, string templateCode,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Template.DeleteTemplate, guildId, templateCode);
        return client.ExecuteAsync<GuildTemplateJsonModel>(route, null, options, cancellationToken);
    }
}