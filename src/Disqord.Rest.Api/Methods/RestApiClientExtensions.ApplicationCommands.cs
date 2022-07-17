using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<ApplicationCommandJsonModel[]> FetchGlobalApplicationCommandsAsync(this IRestApiClient client,
        Snowflake applicationId,
        bool withLocalizations = false,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        KeyValuePair<string, object>[]? queryParameters = null;
        if (withLocalizations)
            queryParameters = new[] { new KeyValuePair<string, object>("with_localizations", true) };

        var route = Format(Route.Interactions.GetGlobalCommands, queryParameters, applicationId);
        return client.ExecuteAsync<ApplicationCommandJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<ApplicationCommandJsonModel> CreateGlobalApplicationCommandAsync(this IRestApiClient client,
        Snowflake applicationId, CreateApplicationCommandJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.CreateGlobalCommand, applicationId);
        return client.ExecuteAsync<ApplicationCommandJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<ApplicationCommandJsonModel> FetchGlobalApplicationCommandAsync(this IRestApiClient client,
        Snowflake applicationId, Snowflake commandId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.GetGlobalCommand, applicationId, commandId);
        return client.ExecuteAsync<ApplicationCommandJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<ApplicationCommandJsonModel> ModifyGlobalApplicationCommandAsync(this IRestApiClient client,
        Snowflake applicationId, Snowflake commandId, ModifyApplicationCommandJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.ModifyGlobalCommand, applicationId, commandId);
        return client.ExecuteAsync<ApplicationCommandJsonModel>(route, content, options, cancellationToken);
    }

    public static Task DeleteGlobalApplicationCommandAsync(this IRestApiClient client,
        Snowflake applicationId, Snowflake commandId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.DeleteGlobalCommand, applicationId, commandId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }

    public static Task<ApplicationCommandJsonModel[]> FetchGuildApplicationCommandsAsync(this IRestApiClient client,
        Snowflake applicationId, Snowflake guildId,
        bool withLocalizations = false,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        KeyValuePair<string, object>[]? queryParameters = null;
        if (withLocalizations)
            queryParameters = new[] { new KeyValuePair<string, object>("with_localizations", true) };

        var route = Format(Route.Interactions.GetGuildCommands, queryParameters, applicationId, guildId);
        return client.ExecuteAsync<ApplicationCommandJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<ApplicationCommandJsonModel[]> SetGlobalApplicationCommandsAsync(this IRestApiClient client,
        Snowflake applicationId, CreateApplicationCommandJsonRestRequestContent[] contents,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.SetGlobalCommands, applicationId);
        return client.ExecuteAsync<ApplicationCommandJsonModel[]>(route, contents.ToObjectContent(), options, cancellationToken);
    }

    public static Task<ApplicationCommandJsonModel> CreateGuildApplicationCommandAsync(this IRestApiClient client,
        Snowflake applicationId, Snowflake guildId, CreateApplicationCommandJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.CreateGuildCommand, applicationId, guildId);
        return client.ExecuteAsync<ApplicationCommandJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<ApplicationCommandJsonModel> FetchGuildApplicationCommandAsync(this IRestApiClient client,
        Snowflake applicationId, Snowflake guildId, Snowflake commandId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.GetGuildCommand, applicationId, guildId, commandId);
        return client.ExecuteAsync<ApplicationCommandJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<ApplicationCommandJsonModel> ModifyGuildApplicationCommandAsync(this IRestApiClient client,
        Snowflake applicationId, Snowflake guildId, Snowflake commandId, ModifyApplicationCommandJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.ModifyGuildCommand, applicationId, guildId, commandId);
        return client.ExecuteAsync<ApplicationCommandJsonModel>(route, content, options, cancellationToken);
    }

    public static Task DeleteGuildApplicationCommandAsync(this IRestApiClient client,
        Snowflake applicationId, Snowflake guildId, Snowflake commandId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.DeleteGuildCommand, applicationId, guildId, commandId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }

    public static Task<ApplicationCommandJsonModel[]> SetGuildApplicationCommandsAsync(this IRestApiClient client,
        Snowflake applicationId, Snowflake guildId, CreateApplicationCommandJsonRestRequestContent[] contents,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.SetGuildCommands, applicationId, guildId);
        return client.ExecuteAsync<ApplicationCommandJsonModel[]>(route, contents.ToObjectContent(), options, cancellationToken);
    }

    public static Task<ApplicationCommandGuildPermissionsJsonModel[]> FetchApplicationCommandsPermissionsAsync(this IRestApiClient client,
        Snowflake applicationId, Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.GetAllCommandPermissions, applicationId, guildId);
        return client.ExecuteAsync<ApplicationCommandGuildPermissionsJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<ApplicationCommandGuildPermissionsJsonModel> FetchApplicationCommandPermissionsAsync(this IRestApiClient client,
        Snowflake applicationId, Snowflake guildId, Snowflake commandId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.GetCommandPermissions, applicationId, guildId, commandId);
        return client.ExecuteAsync<ApplicationCommandGuildPermissionsJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<ApplicationCommandGuildPermissionsJsonModel> SetApplicationCommandPermissionsAsync(this IRestApiClient client,
        Snowflake applicationId, Snowflake guildId, Snowflake commandId, SetApplicationCommandPermissionsJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.SetCommandPermissions, applicationId, guildId, commandId);
        return client.ExecuteAsync<ApplicationCommandGuildPermissionsJsonModel>(route, content, options, cancellationToken);
    }
}