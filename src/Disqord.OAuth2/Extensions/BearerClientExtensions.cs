using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest;
using Disqord.Rest.Api;
using Disqord.Rest.Pagination;
using Qommon.Collections.ReadOnly;

namespace Disqord.OAuth2;

/// <summary>
///     Defines the extension methods for <see cref="IBearerClient"/>.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class BearerClientExtensions
{
    /// <summary>
    ///     Fetches the user tied to this bearer token.
    /// </summary>
    /// <param name="client"> The bearer client. </param>
    /// <param name="options"> The optional request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the request
    ///     that wraps the returned <see cref="ICurrentUser"/>.
    /// </returns>
    public static Task<ICurrentUser> FetchCurrentUserAsync(this IBearerClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.RestClient.FetchCurrentUserAsync(options, cancellationToken);
    }

    /// <summary>
    ///     Fetches authorization information for this bearer token.
    /// </summary>
    /// <param name="client"> The bearer client. </param>
    /// <param name="options"> The optional request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the request
    ///     that wraps the returned <see cref="IBearerAuthorization"/>.
    /// </returns>
    public static async Task<IBearerAuthorization> FetchCurrentAuthorizationAsync(this IBearerClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.RestClient.ApiClient.FetchCurrentAuthorizationAsync(options, cancellationToken).ConfigureAwait(false);
        return new TransientBearerAuthorization(client.RestClient, model);
    }

    /// <summary>
    ///     Enumerates the guilds of the user tied to this bearer token.
    /// </summary>
    /// <param name="client"> The bearer client. </param>
    /// <param name="limit"> The amount of guilds to enumerate. </param>
    /// <param name="fetchDirection"> The direction to enumerate the guilds in. </param>
    /// <param name="startFromId"> The ID of the guild to start enumerating from. </param>
    /// <param name="options"> The optional request options. </param>
    /// <returns>
    ///     A paged enumerable yielding the requested guilds.
    /// </returns>
    public static IPagedEnumerable<IPartialGuild> EnumerateGuilds(this IBearerClient client,
        int limit, FetchDirection fetchDirection = FetchDirection.After, Snowflake? startFromId = null,
        IRestRequestOptions? options = null)
    {
        return client.RestClient.EnumerateGuilds(limit, fetchDirection, startFromId, options);
    }

    /// <summary>
    ///     Fetches the guilds of the user tied to this bearer token.
    /// </summary>
    /// <param name="client"> The bearer client. </param>
    /// <param name="limit"> The amount of guilds to fetch. </param>
    /// <param name="fetchDirection"> The direction to fetch the guilds in. </param>
    /// <param name="startFromId"> The ID of the guild to start fetching from. </param>
    /// <param name="options"> The optional request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the request
    ///     that wraps the returned <see cref="IReadOnlyList{T}"/> of <see cref="IPartialGuild"/>.
    /// </returns>
    public static Task<IReadOnlyList<IPartialGuild>> FetchGuildsAsync(this IBearerClient client,
        int limit = Discord.Limits.Rest.FetchGuildsPageSize, FetchDirection fetchDirection = FetchDirection.Before, Snowflake? startFromId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.RestClient.FetchGuildsAsync(limit, fetchDirection, startFromId, options, cancellationToken);
    }

    /// <summary>
    ///     Fetches the current member from a guild tied to this bearer token.
    /// </summary>
    /// <param name="client"> The bearer client. </param>
    /// <param name="guildId"> The ID of the guild to fetch the member from. </param>
    /// <param name="options"> The optional request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the request
    ///     that wraps the returned <see cref="IMember"/>.
    /// </returns>
    public static async Task<IMember> FetchCurrentGuildMemberAsync(this IBearerClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.RestClient.ApiClient.FetchCurrentGuildMemberAsync(guildId, options, cancellationToken).ConfigureAwait(false);
        return new TransientMember(client.RestClient, guildId, model);
    }

    /// <summary>
    ///     Fetches the connections of the user tied to this bearer token.
    /// </summary>
    /// <param name="client"> The bearer client. </param>
    /// <param name="options"> The optional request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the request
    ///     that wraps the returned <see cref="IReadOnlyList{T}"/> of <see cref="IUserConnection"/>.
    /// </returns>
    public static async Task<IReadOnlyList<IUserConnection>> FetchConnectionsAsync(this IBearerClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var models = await client.RestClient.ApiClient.FetchConnectionsAsync(options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList(client, (model, client) => new TransientUserConnection(client.RestClient, model));
    }

    public static async Task<IApplicationCommandGuildPermissions> SetApplicationCommandPermissionsAsync(this IBearerClient client,
        Snowflake applicationId, Snowflake guildId, Snowflake commandId,
        LocalApplicationCommandPermission[] permissions,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var content = new SetApplicationCommandPermissionsJsonRestRequestContent
        {
            Permissions = permissions.Select(x => x.ToModel()).ToArray()
        };

        var model = await client.RestClient.ApiClient.SetApplicationCommandPermissionsAsync(applicationId, guildId, commandId, content, options, cancellationToken).ConfigureAwait(false);
        return new TransientApplicationCommandGuildPermissions(client.RestClient, model);
    }
}
