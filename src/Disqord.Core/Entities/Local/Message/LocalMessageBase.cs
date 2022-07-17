using System.Collections.Generic;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a message base that can be sent.
/// </summary>
public abstract class LocalMessageBase : ILocalConstruct<LocalMessageBase>
{
    /// <summary>
    ///     Gets or sets the content of this message.
    /// </summary>
    /// <remarks>
    ///     One of <see cref="Content"/>, <see cref="Embeds"/>, <see cref="Attachments"/>, <see cref="Components"/>
    /// </remarks>
    public Optional<string?> Content { get; set; }

    /// <summary>
    ///     Gets or sets whether this message is text-to-speech.
    /// </summary>
    public Optional<bool> IsTextToSpeech { get; set; }

    /// <summary>
    ///     Gets or sets the embeds of this message.
    /// </summary>
    public Optional<IList<LocalEmbed>> Embeds { get; set; }

    /// <summary>
    ///     Gets or sets the flags of this message.
    /// </summary>
    public Optional<MessageFlags> Flags { get; set; }

    /// <summary>
    ///     Gets or sets the allowed mentions of this message.
    /// </summary>
    public Optional<LocalAllowedMentions> AllowedMentions { get; set; }

    /// <summary>
    ///     Gets or sets the attachments of this message.
    /// </summary>
    public Optional<IList<LocalAttachment>> Attachments { get; set; }

    /// <summary>
    ///     Gets or sets the components of this message.
    /// </summary>
    public Optional<IList<LocalRowComponent>> Components { get; set; }

    /// <summary>
    ///     Gets or sets the IDs of the stickers of this message.
    /// </summary>
    public Optional<IList<Snowflake>> StickerIds { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalMessageBase"/>.
    /// </summary>
    protected LocalMessageBase()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalMessageBase"/> from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalMessageBase(LocalMessageBase other)
    {
        Content = other.Content;
        IsTextToSpeech = other.IsTextToSpeech;
        Embeds = other.Embeds.DeepClone();
        Flags = other.Flags;
        AllowedMentions = other.AllowedMentions.Clone();
        Attachments = other.Attachments.DeepClone();
        Components = other.Components.DeepClone();
        StickerIds = other.StickerIds.Clone();
    }

    public abstract LocalMessageBase Clone();

    // public virtual void Validate()
    // {
    //     if (Content == null && _embeds.Count == 0 && _attachments.Count == 0 && _components.Count == 0 && _stickerIds.Count == 0)
    //         throw new InvalidOperationException("A message must contain at least one of content, embeds, attachments, components, or sticker IDs.");
    //
    //     if (_embeds.Sum(x => x.Length) > MaxEmbeddedContentLength)
    //         throw new InvalidOperationException($"The total length of embeds must not exceed {MaxEmbeddedContentLength} characters.");
    //
    //     if (_stickerIds.Count > MaxStickerIdAmount)
    //         throw new InvalidOperationException($"The count of sticker IDs must not exceed {MaxStickerIdAmount}.");
    //
    //     for (var i = 0; i < _embeds.Count; i++)
    //         _embeds[i].Validate();
    //
    //     AllowedMentions?.Validate();
    // }
}
