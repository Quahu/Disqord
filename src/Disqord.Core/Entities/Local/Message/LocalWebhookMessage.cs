using Qommon;

namespace Disqord;

/// <summary>
///     Represents a message that can be sent within a channel by a webhook.
/// </summary>
public class LocalWebhookMessage : LocalMessageBase
{
    /// <summary>
    ///     Gets or sets the name of the author of this message.
    /// </summary>
    public Optional<string> AuthorName { get; set; }

    /// <summary>
    ///     Gets or sets the avatar URL of the author of this message.
    /// </summary>
    public Optional<string> AuthorAvatarUrl { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalWebhookMessage"/>.
    /// </summary>
    public LocalWebhookMessage()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalWebhookMessage"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalWebhookMessage(LocalWebhookMessage other)
        : base(other)
    {
        AuthorName = other.AuthorName;
        AuthorAvatarUrl = other.AuthorAvatarUrl;
    }

    /// <inheritdoc/>
    public override LocalWebhookMessage Clone()
    {
        return new(this);
    }
}
