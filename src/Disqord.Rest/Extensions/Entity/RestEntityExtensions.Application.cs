using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static Task<IReadOnlyList<IApplicationEmoji>> FetchEmojisAsync(this IApplication application,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.FetchApplicationEmojisAsync(application.Id, options, cancellationToken);
    }

    public static Task<IApplicationEmoji> FetchEmojiAsync(this IApplication application,
        Snowflake emojiId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.FetchApplicationEmojiAsync(application.Id, emojiId, options, cancellationToken);
    }

    public static Task<IApplicationEmoji> CreateEmojiAsync(this IApplication application,
        string name, Stream image,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.CreateApplicationEmojiAsync(application.Id, name, image, options, cancellationToken);
    }

    public static Task<IApplicationEmoji> ModifyEmojiAsync(this IApplication application,
        Snowflake emojiId, Action<ModifyApplicationEmojiActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.ModifyApplicationEmojiAsync(application.Id, emojiId, action, options, cancellationToken);
    }

    public static Task DeleteEmojiAsync(this IApplication application,
        Snowflake emojiId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.DeleteApplicationEmojiAsync(application.Id, emojiId, options, cancellationToken);
    }
}