using System;
using System.IO;
using Disqord.Http;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateGuildStickerMultipartRestRequestContent : MultipartRestRequestContent
    {
        public string Name;

        public string Description;

        public string Tags;

        public Stream File;

        public StickerFormatType FileType;

        public override HttpRequestContent CreateHttpContent(IJsonSerializer serializer, IRestRequestOptions options = null)
        {
            Add(Name, "name");
            Add(Description ?? "", "description");
            Add(Tags, "tags");
            var (extension, contentType) = FileType switch
            {
                StickerFormatType.Png or StickerFormatType.APng => ("png", "image/png"),
                StickerFormatType.Lottie => ("json", "application/json"),
                _ => throw new InvalidOperationException("Invalid sticker format type.")
            };
            var content = Add(File, "file", $"file.{extension}");
            content.Headers["Content-Type"] = contentType;
            return base.CreateHttpContent(serializer, options);
        }
    }
}
