using System.ComponentModel;

namespace Disqord.Rest;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class RestRequestOptionsExtensions
{
    public static TOptions WithReason<TOptions>(this TOptions options, string reason)
        where TOptions : IRestRequestOptions
    {
        options.Reason = reason;
        return options;
    }

    public static TOptions WithHeader<TOptions>(this TOptions options, string name, string value)
        where TOptions : IRestRequestOptions
    {
        options.Headers[name] = value;
        return options;
    }
}
