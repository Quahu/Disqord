using System.Collections.Generic;
using System.Linq;
using Disqord.Http;
using Disqord.Http.Default;
using Disqord.Models;
using Disqord.Serialization.Json;
using Disqord.Serialization.Json.Default;
using Qommon;

namespace Disqord.Rest.Api;

public class AttachmentJsonPayloadRestRequestContent<T> : MultipartRestRequestContent
    where T : JsonModelRestRequestContent, IAttachmentRestRequestContent
{
    public T? Payload { get; }

    public LocalAttachment[] Attachments { get; }

    // Hack for the log in Validate().
    private DefaultJsonSerializer? _serializer;

    public AttachmentJsonPayloadRestRequestContent(T payload, IEnumerable<LocalAttachment> attachments)
    {
        Payload = payload;
        Attachments = attachments.ToArray();
    }

    /// <inheritdoc/>
    public override HttpRequestContent CreateHttpContent(IJsonSerializer serializer, IRestRequestOptions? options = null)
    {
        _serializer = serializer as DefaultJsonSerializer;

        var attachments = Attachments;
        List<PartialAttachmentJsonModel>? attachmentModels = null;
        for (var i = 0; i < attachments.Length; i++)
        {
            var attachment = attachments[i];
            var content = new StreamHttpRequestContent(attachment.Stream.Value);
            var name = $"files[{i}]";
            var fileName = attachment.IsSpoiler.GetValueOrDefault()
                ? string.Concat(LocalAttachment.SpoilerPrefix, attachment.FileName.Value)
                : attachment.FileName.Value;

            if (attachment.Description.TryGetValue(out var description) && !string.IsNullOrWhiteSpace(description))
            {
                attachmentModels ??= new List<PartialAttachmentJsonModel>();
                attachmentModels.Add(new PartialAttachmentJsonModel
                {
                    Id = new((ulong) i),
                    Description = description
                });
            }

            FormData.Add((content, name, fileName));
        }

        if (Payload != null)
        {
            if (attachmentModels != null)
            {
                Payload.Attachments = attachmentModels;
            }

            FormData.Insert(0, (Payload.CreateHttpContent(serializer, options), "payload_json", null));
        }

        return base.CreateHttpContent(serializer, options);
    }

    /// <inheritdoc/>
    public override void Validate()
    {
        base.Validate();

        foreach (var attachment in Attachments)
        {
            OptionalGuard.HasValue(attachment.Stream);
            OptionalGuard.HasValue(attachment.FileName);

            var stream = attachment.Stream.Value;
            Guard.CanRead(stream);

            if (stream.CanSeek && stream.Length != 0 && stream.Position == stream.Length)
                Throw.InvalidDataException("The attachment stream's position is the same as its length. Did you forget to rewind it?");

            // See CheckStreamType for more info.
            // Arguably this isn't "correct" to call it here as this isn't JSON serialization
            // but it doesn't matter and warning the user is more important to me.
            var streamConverter = (_serializer?.UnderlyingSerializer.ContractResolver as ContractResolver)?._streamConverter;
            streamConverter?.CheckStreamType(stream);
        }
    }
}
