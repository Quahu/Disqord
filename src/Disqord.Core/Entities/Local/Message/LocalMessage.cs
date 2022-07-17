using Qommon;

namespace Disqord;

/// <summary>
///     Represents a local message that can be sent within a channel by the bot.
/// </summary>
public class LocalMessage : LocalMessageBase, ILocalConstruct<LocalMessage>
{
    /// <summary>
    ///     Gets or sets the message referenced by this message.
    /// </summary>
    /// <remarks>
    ///     This can be used for replying to a message.
    ///     See <see cref="LocalMessageExtensions.WithReply{TMessage}"/>.
    /// </remarks>
    public Optional<LocalMessageReference> Reference { get; set; }

    /// <summary>
    ///     Gets or sets the nonce of this message.
    /// </summary>
    /// <remarks>
    ///     This can be set to then identify the message when receiving it.
    ///     See <see cref="IUserMessage.Nonce"/>.
    /// </remarks>
    public Optional<string> Nonce { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalMessage"/>.
    /// </summary>
    public LocalMessage()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalMessage"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalMessage(LocalMessage other)
        : base(other)
    {
        Reference = other.Reference.Clone();
        Nonce = other.Nonce;
    }

    /// <inheritdoc/>
    public override LocalMessage Clone()
    {
        return new(this);
    }
}
