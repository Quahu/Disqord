using System.IO;
using Disqord.Http;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class CreateGuildStickerMultipartRestRequestContent : MultipartRestRequestContent
{
    public string Name = null!;

    public string? Description;

    public string Tags = null!;

    public Stream File = null!;

    public StickerFormatType FileType;

    /// <inheritdoc/>
    public override HttpRequestContent CreateHttpContent(IJsonSerializer serializer, IRestRequestOptions? options = null)
    {
        Add(Name, "name");
        Add(Description ?? "", "description");
        Add(Tags, "tags");
        var (extension, contentType) = FileType switch
        {
            StickerFormatType.Png or StickerFormatType.APng => ("png", "image/png"),
            StickerFormatType.Lottie => ("json", "application/json"),
            _ => Throw.InvalidOperationException<(string, string)>("Invalid sticker format type.")
        };

        var content = Add(File, "file", $"file.{extension}");
        content.Headers["Content-Type"] = contentType;
        return base.CreateHttpContent(serializer, options);
    }
}
