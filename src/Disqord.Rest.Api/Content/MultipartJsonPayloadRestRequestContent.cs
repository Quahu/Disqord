using System.Collections.Generic;
using System.Linq;
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

        public LocalAttachment[] Attachments { get; }

        // Hack for the log in Validate().
        private DefaultJsonSerializer _serializer;

        public MultipartJsonPayloadRestRequestContent(T payload, IEnumerable<LocalAttachment> attachments)
        {
            Payload = payload;
            Attachments = attachments.ToArray();
        }

        public override HttpRequestContent CreateHttpContent(IJsonSerializer serializer, IRestRequestOptions options = null)
        {
            _serializer = serializer as DefaultJsonSerializer;

            if (Payload != null)
                FormData.Add((Payload.CreateHttpContent(serializer, options), "payload_json", null));

            for (var i = 0; i < Attachments.Length; i++)
            {
                var attachment = Attachments[i];
                var content = new StreamHttpRequestContent(attachment.Stream);
                var name = $"file{i}";
                var fileName = attachment.GetFileName();
                FormData.Add((content, name, fileName));
            }

            return base.CreateHttpContent(serializer, options);
        }

        public override void Validate()
        {
            base.Validate();

            foreach (var attachment in Attachments)
            {
                var stream = attachment.Stream;
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
}
