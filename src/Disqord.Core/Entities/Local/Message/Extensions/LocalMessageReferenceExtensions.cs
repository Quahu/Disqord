using System.ComponentModel;

namespace Disqord;

/// <summary>
///     Defines <see cref="LocalMessageReference"/> extensions.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalMessageReferenceExtensions
{
    /// <summary>
    ///     Sets the ID of the referenced message.
    /// </summary>
    /// <param name="messageReference"> The <see cref="LocalMessageReference"/> instance. </param>
    /// <param name="messageId"> The ID of the referenced message. </param>
    /// <typeparam name="TMessageReference"> The <see cref="LocalMessageReference"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    /// <seealso cref="LocalMessageReference.MessageId"/>
    public static TMessageReference WithMessageId<TMessageReference>(this TMessageReference messageReference, Snowflake messageId)
        where TMessageReference : LocalMessageReference
    {
        messageReference.MessageId = messageId;
        return messageReference;
    }

    /// <summary>
    ///     Sets the ID of the referenced message's channel.
    /// </summary>
    /// <param name="messageReference"> The <see cref="LocalMessageReference"/> instance. </param>
    /// <param name="channelId"> The ID of the referenced message's channel. </param>
    /// <typeparam name="TMessageReference"> The <see cref="LocalMessageReference"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    /// <seealso cref="LocalMessageReference.ChannelId"/>
    public static TMessageReference WithChannelId<TMessageReference>(this TMessageReference messageReference, Snowflake channelId)
        where TMessageReference : LocalMessageReference
    {
        messageReference.ChannelId = channelId;
        return messageReference;
    }

    /// <summary>
    ///     Sets the ID of the referenced message's guild.
    /// </summary>
    /// <param name="messageReference"> The <see cref="LocalMessageReference"/> instance. </param>
    /// <param name="guildId"> The ID of the referenced message's guild. </param>
    /// <typeparam name="TMessageReference"> The <see cref="LocalMessageReference"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    /// <seealso cref="LocalMessageReference.GuildId"/>
    public static TMessageReference WithGuildId<TMessageReference>(this TMessageReference messageReference, Snowflake guildId)
        where TMessageReference : LocalMessageReference
    {
        messageReference.GuildId = guildId;
        return messageReference;
    }

    /// <summary>
    ///     Sets the mention types Discord will parse from the message's content.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="LocalMessageReference.FailOnUnknownMessage"/>
    /// </remarks>
    /// <param name="messageReference"> The <see cref="LocalMessageReference"/> instance. </param>
    /// <param name="failOnUnknownMessage"> The parsed mentions. </param>
    /// <typeparam name="TMessageReference"> The <see cref="LocalMessageReference"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    /// <seealso cref="LocalMessageReference.FailOnUnknownMessage"/>
    public static TMessageReference WithFailOnUnknownMessage<TMessageReference>(this TMessageReference messageReference, bool failOnUnknownMessage = true)
        where TMessageReference : LocalMessageReference
    {
        messageReference.FailOnUnknownMessage = failOnUnknownMessage;
        return messageReference;
    }
}
