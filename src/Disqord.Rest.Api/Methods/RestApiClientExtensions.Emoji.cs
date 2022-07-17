using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<EmojiJsonModel[]> FetchGuildEmojisAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Emoji.GetGuildEmojis, guildId);
        return client.ExecuteAsync<EmojiJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<EmojiJsonModel> FetchGuildEmojiAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake emojiId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Emoji.GetGuildEmoji, guildId, emojiId);
        return client.ExecuteAsync<EmojiJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<EmojiJsonModel> CreateGuildEmojiAsync(this IRestApiClient client,
        Snowflake guildId, CreateGuildEmojiJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Emoji.CreateGuildEmoji, guildId);
        return client.ExecuteAsync<EmojiJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<EmojiJsonModel> ModifyGuildEmojiAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake emojiId, ModifyGuildEmojiJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Emoji.ModifyGuildEmoji, guildId, emojiId);
        return client.ExecuteAsync<EmojiJsonModel>(route, content, options, cancellationToken);
    }

    public static Task DeleteGuildEmojiAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake emojiId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Emoji.DeleteGuildEmoji, guildId, emojiId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }
}