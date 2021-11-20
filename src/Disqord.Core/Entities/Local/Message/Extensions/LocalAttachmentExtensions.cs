using System.ComponentModel;
using System.IO;

namespace Disqord
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class LocalAttachmentExtensions
    {
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
}
