using System.Collections.Generic;
using System.IO;
using Disqord.Http;
using Disqord.Http.Default;
using Disqord.Serialization.Json;
using Disqord.Serialization.Json.Default;
using Qommon;

namespace Disqord.Rest.Api
{
    public class MultipartJsonPayloadRestRequestContent<T> : MultipartRestRequestContent
        where T : JsonModelRestRequestContent
    {
        public T Payload { get; }

        public IEnumerable<LocalAttachment> Attachments { get; }

        public MultipartJsonPayloadRestRequestContent(T payload, IEnumerable<LocalAttachment> attachments)
        {
            Payload = payload;
            Attachments = attachments;
        }

        public override HttpRequestContent CreateHttpContent(IJsonSerializer serializer, IRestRequestOptions options = null)
        {
            if (Payload != null)
                FormData.Add((Payload.CreateHttpContent(serializer, options), "payload_json", null));

            var counter = 0;
            foreach (var attachment in Attachments)
            {
                ValidateStream(serializer, attachment.Stream);
                FormData.Add((new StreamHttpRequestContent(attachment.Stream), $"file{counter++}", attachment.GetFileName()));
            }

            return base.CreateHttpContent(serializer, options);
        }

        private static void ValidateStream(IJsonSerializer serializer, Stream stream)
        {
            Guard.CanRead(stream);

            if (stream.CanSeek && stream.Length != 0 && stream.Position == stream.Length)
                Throw.InvalidDataException("The attachment stream's position is the same as its length. Did you forget to rewind it?");

            if (serializer is DefaultJsonSerializer jsonSerializer)
            {
                // See CheckStreamType for more info.
                // Arguably this isn't "correct" to call it here as this isn't JSON serialization
                // but it doesn't matter and warning the user is more important to me.
                var streamConverter = (jsonSerializer.UnderlyingSerializer.ContractResolver as ContractResolver)._streamConverter;
                streamConverter.CheckStreamType(stream);
            }
        }
    }
}
