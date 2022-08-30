using Disqord.Models;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a local partial attachment that can be sent within a message.
/// </summary>
/// <remarks>
///     This type can be used for modifying existing attachments.
/// </remarks>
public class LocalPartialAttachment : ILocalConstruct<LocalPartialAttachment>, IJsonConvertible<PartialAttachmentJsonModel>
{
    /// <summary>
    ///     Gets or sets the ID of this attachment.
    /// </summary>
    /// <remarks>
    ///     This is required for modifying attachments on an existing message.
    /// </remarks>
    public Optional<Snowflake> Id { get; set; }

    /// <summary>
    ///     Gets or sets the description of this attachment.
    /// </summary>
    public Optional<string> Description { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalPartialAttachment"/>.
    /// </summary>
    public LocalPartialAttachment()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalPartialAttachment"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalPartialAttachment(LocalPartialAttachment other)
    {
        Id = other.Id;
        Description = other.Description;
    }

    /// <summary>
    ///     Instantiates a new <see cref="LocalPartialAttachment"/>.
    /// </summary>
    /// <param name="id"> The ID of the attachment. </param>
    public LocalPartialAttachment(Snowflake id)
    {
        Id = id;
    }

    /// <inheritdoc/>
    public virtual LocalPartialAttachment Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public virtual PartialAttachmentJsonModel ToModel()
    {
        return new AttachmentJsonModel
        {
            Id = Id.Value,
            Description = Description
        };
    }
}
