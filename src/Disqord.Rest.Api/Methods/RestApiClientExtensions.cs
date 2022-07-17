using System.Collections.Generic;
using System.ComponentModel;

namespace Disqord.Rest.Api;

[EditorBrowsable(EditorBrowsableState.Never)]
public static partial class RestApiClientExtensions
{
    private static IFormattedRoute Format(Route route, params object[]? arguments)
    {
        return route.Format(arguments);
    }

    private static IFormattedRoute Format(Route route, IEnumerable<KeyValuePair<string, object>>? queryParameters, params object[]? arguments)
    {
        return route.Format(arguments, queryParameters);
    }
}
