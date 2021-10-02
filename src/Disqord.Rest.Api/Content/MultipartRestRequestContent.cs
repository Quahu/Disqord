using System;
using System.Collections.Generic;
using System.IO;
using Disqord.Http;
using Disqord.Http.Default;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class MultipartRestRequestContent : IRestRequestContent
    {
        public List<(HttpRequestContent Content, string Name, string FileName)> FormData { get; }

        public MultipartRestRequestContent()
        {
            FormData = new List<(HttpRequestContent Content, string Name, string FileName)>();
        }

        public StringHttpRequestContent Add(string text, string name)
        {
            var content = new StringHttpRequestContent(text);
            FormData.Add((content, name, null));
            return content;
        }

        public StreamHttpRequestContent Add(Stream stream, string name, string fileName = null)
        {
            var content = new StreamHttpRequestContent(stream);
            FormData.Add((content, name, fileName));
            return content;
        }

        public virtual HttpRequestContent CreateHttpContent(IJsonSerializer serializer, IRestRequestOptions options = null)
        {
            var content = new MultipartFormDataHttpRequestContent($"---------{DateTime.Now}--");
            content.FormData.AddRange(FormData);
            return content;
        }

        public virtual void Validate()
        {
            foreach (var (content, name, _) in FormData)
            {
                Guard.IsNotNull(content);
                Guard.IsNotNullOrWhiteSpace(name);
            }
        }
    }
}
