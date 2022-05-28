using System.Collections.Generic;
using Disqord.Http;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class JsonObjectRestRequestContent<T> : IRestRequestContent
    {
        public T Object { get; }

        public JsonObjectRestRequestContent(T obj)
        {
            Object = obj;
        }

        /// <inheritdoc/>
        public HttpRequestContent CreateHttpContent(IJsonSerializer serializer, IRestRequestOptions options = null)
            => JsonModelRestRequestContent.FromObject(Object, serializer);

        /// <inheritdoc/>
        public virtual void Validate()
        {
            if (Object is JsonModel)
            {
                (Object as JsonModel).Validate();
            }
            else if (Object is IEnumerable<JsonModel> jsonModels)
            {
                foreach (var jsonModel in jsonModels)
                {
                    Guard.IsNotNull(jsonModel);

                    jsonModel.Validate();
                }
            }
        }

        public static implicit operator JsonObjectRestRequestContent<T>(T obj)
            => new(obj);
    }
}
