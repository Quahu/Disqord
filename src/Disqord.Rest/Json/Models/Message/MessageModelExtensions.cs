namespace Disqord.Models
{
    internal static partial class ModelExtensions
    {
        public static Attachment ToAttachment(this AttachmentModel model)
            => new Attachment
            {
                Id = model.Id,
                FileName = model.FileName,
                Url = model.Url,
                ProxyUrl = model.ProxyUrl,
                Size = model.Size,
                Height = model.Height,
                Width = model.Width
            };

        public static MessageActivity ToActivity(this MessageActivityModel model)
            => new MessageActivity(model);

        public static MessageApplication ToApplication(this MessageApplicationModel model)
            => new MessageApplication(model);

        public static MessageReference ToReference(this MessageReferenceModel model)
            => new MessageReference(model);
    }
}
