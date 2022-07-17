using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest;

public static partial class RestClientExtensions
{
    public static async Task<IReadOnlyList<IApplicationCommand>> FetchGlobalApplicationCommandsAsync(this IRestClient client,
        Snowflake applicationId,
        bool withLocalizations = false,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var models = await client.ApiClient.FetchGlobalApplicationCommandsAsync(applicationId, withLocalizations, options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList(client, (model, client) => TransientApplicationCommand.Create(client, model));
    }

    public static async Task<IApplicationCommand> CreateGlobalApplicationCommandAsync(this IRestClient client,
        Snowflake applicationId, LocalApplicationCommand command,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var content = command.ToContent(client.ApiClient.Serializer);
        var model = await client.ApiClient.CreateGlobalApplicationCommandAsync(applicationId, content, options, cancellationToken).ConfigureAwait(false);
        return TransientApplicationCommand.Create(client, model);
    }

    public static async Task<IApplicationCommand> FetchGlobalApplicationCommandAsync(this IRestClient client,
        Snowflake applicationId, Snowflake commandId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchGlobalApplicationCommandAsync(applicationId, commandId, options, cancellationToken).ConfigureAwait(false);
        return TransientApplicationCommand.Create(client, model);
    }

    public static async Task<IApplicationCommand> ModifyGlobalApplicationCommandAsync(this IRestClient client,
        Snowflake applicationId, Snowflake commandId, Action<ModifyApplicationCommandActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var content = action.ToContent(client.ApiClient.Serializer);
        var model = await client.ApiClient.ModifyGlobalApplicationCommandAsync(applicationId, commandId, content, options, cancellationToken).ConfigureAwait(false);
        return TransientApplicationCommand.Create(client, model);
    }

    public static Task DeleteGlobalApplicationCommandAsync(this IRestClient client,
        Snowflake applicationId, Snowflake commandId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.ApiClient.DeleteGlobalApplicationCommandAsync(applicationId, commandId, options, cancellationToken);
    }

    public static async Task<IReadOnlyList<IApplicationCommand>> SetGlobalApplicationCommandsAsync(this IRestClient client,
        Snowflake applicationId, IEnumerable<LocalApplicationCommand> commands,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var contents = commands.Select(command => command.ToContent(client.ApiClient.Serializer)).ToArray();
        var models = await client.ApiClient.SetGlobalApplicationCommandsAsync(applicationId, contents, options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList(client, (model, client) => TransientApplicationCommand.Create(client, model));
    }

    public static async Task<IReadOnlyList<IApplicationCommand>> FetchGuildApplicationCommandsAsync(this IRestClient client,
        Snowflake applicationId, Snowflake guildId,
        bool withLocalizations = false,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var models = await client.ApiClient.FetchGuildApplicationCommandsAsync(applicationId, guildId, withLocalizations, options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList(client, (model, client) => TransientApplicationCommand.Create(client, model));
    }

    public static async Task<IApplicationCommand> CreateGuildApplicationCommandAsync(this IRestClient client,
        Snowflake applicationId, Snowflake guildId, LocalApplicationCommand command,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var content = command.ToContent(client.ApiClient.Serializer);
        var model = await client.ApiClient.CreateGuildApplicationCommandAsync(applicationId, guildId, content, options, cancellationToken).ConfigureAwait(false);
        return TransientApplicationCommand.Create(client, model);
    }

    public static async Task<IApplicationCommand> FetchGuildApplicationCommandAsync(this IRestClient client,
        Snowflake applicationId, Snowflake guildId, Snowflake commandId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchGuildApplicationCommandAsync(applicationId, guildId, commandId, options, cancellationToken).ConfigureAwait(false);
        return TransientApplicationCommand.Create(client, model);
    }

    public static async Task<IApplicationCommand> ModifyGuildApplicationCommandAsync(this IRestClient client,
        Snowflake applicationId, Snowflake guildId, Snowflake commandId, Action<ModifyApplicationCommandActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var content = action.ToContent(client.ApiClient.Serializer);
        var model = await client.ApiClient.ModifyGuildApplicationCommandAsync(applicationId, guildId, commandId, content, options, cancellationToken).ConfigureAwait(false);
        return TransientApplicationCommand.Create(client, model);
    }

    public static Task DeleteGuildApplicationCommandAsync(this IRestClient client,
        Snowflake applicationId, Snowflake guildId, Snowflake commandId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.ApiClient.DeleteGuildApplicationCommandAsync(applicationId, guildId, commandId, options, cancellationToken);
    }

    public static async Task<IReadOnlyList<IApplicationCommand>> SetGuildApplicationCommandsAsync(this IRestClient client,
        Snowflake applicationId, Snowflake guildId, IEnumerable<LocalApplicationCommand> commands,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var contents = commands.Select(command => command.ToContent(client.ApiClient.Serializer)).ToArray();
        var models = await client.ApiClient.SetGuildApplicationCommandsAsync(applicationId, guildId, contents, options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList(client, (model, client) => TransientApplicationCommand.Create(client, model));
    }

    public static async Task<IReadOnlyList<IApplicationCommandGuildPermissions>> FetchApplicationCommandsPermissionsAsync(this IRestClient client,
        Snowflake applicationId, Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var models = await client.ApiClient.FetchApplicationCommandsPermissionsAsync(applicationId, guildId, options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList(client, (model, client) => new TransientApplicationCommandGuildPermissions(client, model));
    }

    public static async Task<IApplicationCommandGuildPermissions> FetchApplicationCommandPermissionsAsync(this IRestClient client,
        Snowflake applicationId, Snowflake guildId, Snowflake commandId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchApplicationCommandPermissionsAsync(applicationId, guildId, commandId, options, cancellationToken).ConfigureAwait(false);
        return new TransientApplicationCommandGuildPermissions(client, model);
    }
}