using System;
using System.ComponentModel;
using Disqord.Rest.Api;
using Disqord.Rest.Api.Default;

namespace Disqord.Rest;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class DefaultRestRequestOptionsExtensions
{
    public static TOptions WithRequestAction<TOptions>(this TOptions options, Action<IRestRequest> action)
        where TOptions : DefaultRestRequestOptions
    {
        options.RequestAction = action;
        return options;
    }

    public static TOptions WithHeadersAction<TOptions>(this TOptions options, Action<DefaultRestResponseHeaders> action)
        where TOptions : DefaultRestRequestOptions
    {
        options.HeadersAction = action;
        return options;
    }
}