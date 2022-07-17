using System.ComponentModel;

namespace Disqord;

/// <summary>
///     Defines <see cref="LocalWebhookMessage"/> extensions.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalWebhookMessageExtensions
{
    /// <summary>
    ///     Sets the name of the author of this message.
    /// </summary>
    /// <param name="message"> The <see cref="LocalWebhookMessage"/> instance. </param>
    /// <param name="authorName"> The name of the author. </param>
    /// <typeparam name="TMessage"> The <see cref="LocalWebhookMessage"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    /// <seealso cref="LocalWebhookMessage.AuthorName"/>
    public static TMessage WithAuthorName<TMessage>(this TMessage message, string authorName)
        where TMessage : LocalWebhookMessage
    {
        message.AuthorName = authorName;
        return message;
    }

    /// <summary>
    ///     Sets the avatar URL of the author of this message.
    /// </summary>
    /// <param name="message"> The <see cref="LocalWebhookMessage"/> instance. </param>
    /// <param name="authorAvatarUrl"> The avatar URL of the author. </param>
    /// <typeparam name="TMessage"> The <see cref="LocalWebhookMessage"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    /// <seealso cref="LocalWebhookMessage.AuthorAvatarUrl"/>
    public static TMessage WithAuthorAvatarUrl<TMessage>(this TMessage message, string authorAvatarUrl)
        where TMessage : LocalWebhookMessage
    {
        message.AuthorAvatarUrl = authorAvatarUrl;
        return message;
    }
}
