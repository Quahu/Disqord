using System;
using Disqord.Http;

namespace Disqord.Rest.Api;

public interface IRestResponse : IDisposable
{
    IHttpResponse HttpResponse { get; }
}