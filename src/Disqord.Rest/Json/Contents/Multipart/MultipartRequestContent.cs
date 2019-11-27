using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class MultipartRequestContent<T> : IRequestContent where T : JsonRequestContent
    {
        public T Content { get; set; }

        public LocalAttachment Attachment { get; set; }

        public IReadOnlyList<LocalAttachment> Attachments { get; set; }

        public HttpContent Prepare(IJsonSerializer serializer, RestRequestOptions options)
        {
            var content = new MultipartFormDataContent($"---------{DateTime.Now}--")
            {
                { Content.Prepare(serializer, options), "payload_json" },
            };

            if (Attachment != null)
            {
                content.Add(new CustomStreamContent(Attachment.Stream), "file", Attachment.FileName);
            }
            else if (Attachments != null)
            {
                for (var i = 0; i < Attachments.Count; i++)
                {
                    var attachment = Attachments[i];
                    content.Add(new CustomStreamContent(attachment.Stream), $"file{i}", attachment.FileName);
                }
            }

            return content;
        }

        private sealed class CustomStreamContent : StreamContent
        {
            public CustomStreamContent(Stream content) : base(content)
            { }

            // Prevents the original stream from being disposed.
            protected override void Dispose(bool disposing)
                => base.Dispose(false);
        }
    }
}
