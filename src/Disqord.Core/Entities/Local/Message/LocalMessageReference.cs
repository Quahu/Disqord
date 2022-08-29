using Disqord.Models;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents the referenced message of a Discord message.<br/>
///     See <a href="https://discord.com/developers/docs/resources/channel#message-reference-object">Discord documentation</a>.
/// </summary>
public class LocalMessageReference : ILocalConstruct<LocalMessageReference>, IJsonConvertible<MessageReferenceJsonModel>
{
    /// <summary>
    ///     Gets or sets the ID of the referenced message.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<Snowflake> MessageId { get; set; }

    /// <summary>
    ///     Gets or sets the ID of the referenced message's channel.
    /// </summary>
    public Optional<Snowflake> ChannelId { get; set; }

    /// <summary>
    ///     Gets or sets the ID of the referenced message's guild.
    /// </summary>
    public Optional<Snowflake> GuildId { get; set; }

    /// <summary>
    ///     Gets or sets whether the message request should fail if the referenced message is not found.
    /// </summary>
    /// <remarks>
    ///     The library defaults this to <see langword="false"/>.<br/>
    ///     This does not prevent errors on invalid <see cref="ChannelId"/> and/or <see cref="GuildId"/>.
    /// </remarks>
    public Optional<bool> FailOnUnknownMessage { get; set; } = false;

    /// <summary>
    ///     Instantiates a new <see cref="LocalMessageReference"/>.
    /// </summary>
    public LocalMessageReference()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalMessageReference"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalMessageReference(LocalMessageReference other)
    {
        MessageId = other.MessageId;
        ChannelId = other.ChannelId;
        GuildId = other.GuildId;
        FailOnUnknownMessage = other.FailOnUnknownMessage;
    }

    /// <inheritdoc/>
    public virtual LocalMessageReference Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public virtual MessageReferenceJsonModel ToModel()
    {
        return new MessageReferenceJsonModel
        {
            MessageId = MessageId,
            ChannelId = ChannelId,
            GuildId = GuildId,
            FailIfNotExists = FailOnUnknownMessage
        };
    }

    /// <summary>
    ///     Converts the specified message reference to a <see cref="LocalMessageReference"/>.
    /// </summary>
    /// <remarks>
    ///     The converted <see cref="LocalMessageReference"/> might not have <see cref="MessageId"/> set
    ///     depending on the input message reference.
    /// </remarks>
    /// <param name="reference"> The message reference to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalMessageReference"/>.
    /// </returns>
    public static LocalMessageReference CreateFrom(IMessageReference reference)
    {
        return new LocalMessageReference
        {
            MessageId = Optional.FromNullable(reference.MessageId),
            ChannelId = reference.ChannelId,
            GuildId = Optional.FromNullable(reference.GuildId)
        };
    }
}
