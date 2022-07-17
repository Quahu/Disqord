using System.ComponentModel;
using System.IO;

namespace Disqord;

/// <summary>
///     Defines <see cref="LocalAttachment"/> extensions.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalAttachmentExtensions
{
    public static TAttachment WithId<TAttachment>(this TAttachment attachment, Snowflake id)
        where TAttachment : LocalAttachment
    {
        attachment.Id = id;
        return attachment;
    }

    public static TAttachment WithStream<TAttachment>(this TAttachment attachment, Stream stream)
        where TAttachment : LocalAttachment
    {
        attachment.Stream = stream;
        return attachment;
    }

    public static TAttachment WithFileName<TAttachment>(this TAttachment attachment, string fileName)
        where TAttachment : LocalAttachment
    {
        attachment.FileName = fileName;
        return attachment;
    }

    public static TAttachment WithDescription<TAttachment>(this TAttachment attachment, string description)
        where TAttachment : LocalAttachment
    {
        attachment.Description = description;
        return attachment;
    }

    public static TAttachment WithIsSpoiler<TAttachment>(this TAttachment attachment, bool isSpoiler = true)
        where TAttachment : LocalAttachment
    {
        attachment.IsSpoiler = isSpoiler;
        return attachment;
    }
}
