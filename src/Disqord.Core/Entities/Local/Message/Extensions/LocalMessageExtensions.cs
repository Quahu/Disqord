using System.ComponentModel;
using Qommon;

namespace Disqord;

/// <summary>
///     Defines <see cref="LocalMessage"/> extensions.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalMessageExtensions
{
    /// <summary>
    ///     Sets the message reference of this message.
    /// </summary>
    /// <param name="message"> The <see cref="LocalMessage"/> instance. </param>
    /// <param name="reference"> The message reference. </param>
    /// <typeparam name="TMessage"> The <see cref="LocalMessage"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    public static TMessage WithReference<TMessage>(this TMessage message, LocalMessageReference reference)
        where TMessage : LocalMessage
    {
        message.Reference = reference;
        return message;
    }

    /// <summary>
    ///     Sets the message reference of this message indicating it is a reply to the message with the specified <paramref name="messageId"/>.
    /// </summary>
    /// <param name="message"> The <see cref="LocalMessage"/> instance. </param>
    /// <param name="messageId"> The ID of the message this message is a reply to. </param>
    /// <param name="channelId"> The ID of the channel of the replied message. This does not have to be set. </param>
    /// <param name="guildId"> The ID of the guild of the replied message. This does not have to be set. </param>
    /// <param name="failOnUnknownMessage"> Whether to fail if the replied to message does not exist. </param>
    /// <typeparam name="TMessage"> The <see cref="LocalMessage"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    public static TMessage WithReply<TMessage>(this TMessage message, Snowflake messageId, Snowflake? channelId = null, Snowflake? guildId = null, bool failOnUnknownMessage = false)
        where TMessage : LocalMessage
    {
        var reference = message.Reference.GetValueOrDefault();
        if (reference == null)
        {
            message.Reference = reference = new LocalMessageReference();
        }

        reference.MessageId = messageId;
        reference.ChannelId = Optional.FromNullable(channelId);
        reference.GuildId = Optional.FromNullable(guildId);
        reference.FailOnUnknownMessage = failOnUnknownMessage;
        return message;
    }

    /// <summary>
    ///     Sets the nonce of this message.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="LocalMessage.Nonce"/>
    /// </remarks>
    /// <param name="message"> The <see cref="LocalMessage"/> instance. </param>
    /// <param name="nonce"> The nonce. </param>
    /// <typeparam name="TMessage"> The <see cref="LocalMessage"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    /// <seealso cref="LocalMessage.Nonce"/>
    public static TMessage WithNonce<TMessage>(this TMessage message, string nonce)
        where TMessage : LocalMessage
    {
        message.Nonce = nonce;
        return message;
    }
}
