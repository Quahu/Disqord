namespace Disqord
{
    internal static partial class ModelExtensions
    {
        public static Attachment ToAttachment(this Models.AttachmentModel model)
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
    }
}
