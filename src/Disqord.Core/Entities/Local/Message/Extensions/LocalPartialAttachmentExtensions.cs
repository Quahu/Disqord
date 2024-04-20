using System.ComponentModel;

namespace Disqord;

/// <summary>
///     Defines <see cref="LocalPartialAttachment"/> extensions.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalPartialAttachmentExtensions
{
    public static TAttachment WithId<TAttachment>(this TAttachment attachment, Snowflake id)
        where TAttachment : LocalPartialAttachment
    {
        attachment.Id = id;
        return attachment;
    }

    public static TAttachment WithDescription<TAttachment>(this TAttachment attachment, string description)
        where TAttachment : LocalPartialAttachment
    {
        attachment.Description = description;
        return attachment;
    }
}
