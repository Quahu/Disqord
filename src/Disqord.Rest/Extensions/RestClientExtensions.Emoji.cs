using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest;

public static partial class RestClientExtensions
{
    public static async Task<IReadOnlyList<IApplicationEmoji>> FetchApplicationEmojisAsync(this IRestClient client,
        Snowflake applicationId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchApplicationEmojisAsync(applicationId, options, cancellationToken).ConfigureAwait(false);
        return model.Items.ToReadOnlyList((client, applicationId), static (model, state) =>
        {
            var (client, applicationId) = state;
            return new TransientApplicationEmoji(client, applicationId, model);
        });
    }

    public static async Task<IApplicationEmoji> FetchApplicationEmojiAsync(this IRestClient client,
        Snowflake applicationId, Snowflake emojiId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchApplicationEmojiAsync(applicationId, emojiId, options, cancellationToken).ConfigureAwait(false);
        return new TransientApplicationEmoji(client, applicationId, model);
    }

    public static async Task<IApplicationEmoji> CreateApplicationEmojiAsync(this IRestClient client,
        Snowflake applicationId, string name, Stream image,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var content = new CreateApplicationEmojiJsonRestRequestContent(name, image);

        var model = await client.ApiClient.CreateApplicationEmojiAsync(applicationId, content, options, cancellationToken).ConfigureAwait(false);
        return new TransientApplicationEmoji(client, applicationId, model);
    }

    public static async Task<IApplicationEmoji> ModifyApplicationEmojiAsync(this IRestClient client,
        Snowflake applicationId, Snowflake emojiId, Action<ModifyApplicationEmojiActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(action);

        var properties = new ModifyApplicationEmojiActionProperties();
        action.Invoke(properties);
        var content = new ModifyApplicationEmojiJsonRestRequestContent()
        {
            Name = properties.Name
        };

        var model = await client.ApiClient.ModifyApplicationEmojiAsync(applicationId, emojiId, content, options, cancellationToken).ConfigureAwait(false);
        return new TransientApplicationEmoji(client, applicationId, model);
    }

    public static Task DeleteApplicationEmojiAsync(this IRestClient client,
        Snowflake applicationId, Snowflake emojiId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.ApiClient.DeleteApplicationEmojiAsync(applicationId, emojiId, options, cancellationToken);
    }

    public static async Task<IReadOnlyList<IGuildEmoji>> FetchGuildEmojisAsync(this IRestClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var models = await client.ApiClient.FetchGuildEmojisAsync(guildId, options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList((client, guildId), static (model, state) =>
        {
            var (client, guildId) = state;
            return new TransientGuildEmoji(client, guildId, model);
        });
    }

    public static async Task<IGuildEmoji> FetchGuildEmojiAsync(this IRestClient client,
        Snowflake guildId, Snowflake emojiId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchGuildEmojiAsync(guildId, emojiId, options, cancellationToken).ConfigureAwait(false);
        return new TransientGuildEmoji(client, guildId, model);
    }

    public static async Task<IGuildEmoji> CreateGuildEmojiAsync(this IRestClient client,
        Snowflake guildId, string name, Stream image, Action<CreateGuildEmojiActionProperties>? action = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var properties = new CreateGuildEmojiActionProperties();
        action?.Invoke(properties);
        var content = new CreateGuildEmojiJsonRestRequestContent(name, image)
        {
            Roles = Optional.Convert(properties.RoleIds, x => x.ToArray())
        };

        var model = await client.ApiClient.CreateGuildEmojiAsync(guildId, content, options, cancellationToken).ConfigureAwait(false);
        return new TransientGuildEmoji(client, guildId, model);
    }

    public static async Task<IGuildEmoji> ModifyGuildEmojiAsync(this IRestClient client,
        Snowflake guildId, Snowflake emojiId, Action<ModifyGuildEmojiActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(action);

        var properties = new ModifyGuildEmojiActionProperties();
        action.Invoke(properties);
        var content = new ModifyGuildEmojiJsonRestRequestContent
        {
            Name = properties.Name,
            Roles = Optional.Convert(properties.RoleIds, x => x.ToArray())
        };

        var model = await client.ApiClient.ModifyGuildEmojiAsync(guildId, emojiId, content, options, cancellationToken).ConfigureAwait(false);
        return new TransientGuildEmoji(client, guildId, model);
    }

    public static Task DeleteGuildEmojiAsync(this IRestClient client,
        Snowflake guildId, Snowflake emojiId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.ApiClient.DeleteGuildEmojiAsync(guildId, emojiId, options, cancellationToken);
    }
}
