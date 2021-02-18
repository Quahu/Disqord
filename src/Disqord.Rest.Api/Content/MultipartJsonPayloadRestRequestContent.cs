using System;
using System.Collections.Generic;
using System.IO;
using Disqord.Http;
using Disqord.Http.Default;

namespace Disqord.Rest.Api
{
    public class MultipartJsonPayloadRestRequestContent<T> : IRestRequestContent
        where T : JsonModelRestRequestContent
    {
        public T Payload { get; }

        public IEnumerable<LocalAttachment> Attachments { get; }

        public MultipartJsonPayloadRestRequestContent(T payload, IEnumerable<LocalAttachment> attachments)
        {
            Payload = payload;
            Attachments = attachments;
        }

        public HttpRequestContent CreateHttpContent(IRestApiClient client, IRestRequestOptions options = null)
        {
            var content = new MultipartFormDataHttpRequestContent($"---------{DateTime.Now}--");
            if (Payload != null)
                content.FormData.Add((Payload.CreateHttpContent(client, options), "payload_json", null));

            var counter = 0;
            foreach (var attachment in Attachments)
            {
                ValidateStream(attachment.Stream);
                content.FormData.Add((new StreamHttpRequestContent(attachment.Stream), $"file{counter++}", attachment.GetFileName()));
            }

            return content;
        }

        private static void ValidateStream(Stream stream)
        {
            if (stream.CanSeek && stream.Length != 0 && stream.Position == stream.Length)
                throw new InvalidDataException("The attachment stream's position is the same as its length. Did you forget to rewind it?");
        }
    }
}
