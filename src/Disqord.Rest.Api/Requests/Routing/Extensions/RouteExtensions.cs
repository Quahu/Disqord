using System.Collections.Generic;
using System.ComponentModel;

namespace Disqord.Rest.Api;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class RouteExtensions
{
    public static IFormattedRoute Format(this IFormattableRoute route,
        object[]? arguments = null, IEnumerable<KeyValuePair<string, object>>? queryParameters = null)
    {
        return new RouteFormatter().Format(route, arguments, queryParameters);
    }

    public static IFormattedRoute Format(this IFormattableRoute route, RouteFormatter formatter,
        object[]? arguments = null, IEnumerable<KeyValuePair<string, object>>? queryParameters = null)
    {
        return formatter.Format(route, arguments, queryParameters);
    }
}
