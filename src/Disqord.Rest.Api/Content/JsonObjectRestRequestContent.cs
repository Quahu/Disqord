using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Disqord.Http;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class JsonObjectRestRequestContent<T> : IRestRequestContent
    where T : notnull
{
    public T Object { get; }

    public JsonObjectRestRequestContent(T obj)
    {
        Object = obj;
    }

    /// <inheritdoc/>
    public HttpRequestContent CreateHttpContent(IJsonSerializer serializer, IRestRequestOptions? options = null)
    {
        return JsonModelRestRequestContent.FromObject(Object, serializer);
    }

    /// <inheritdoc/>
    public virtual void Validate()
    {
        var obj = Object;
        if (obj is JsonModel)
        {
            Unsafe.As<JsonModel>(obj).Validate();
        }
        else if (obj is IEnumerable<JsonModel>)
        {
            foreach (var jsonModel in Unsafe.As<IEnumerable<JsonModel>>(obj))
            {
                Guard.IsNotNull(jsonModel);

                jsonModel.Validate();
            }
        }
    }

    public static implicit operator JsonObjectRestRequestContent<T>(T obj)
    {
        return new(obj);
    }
}
