using System.Collections.Generic;
using System.ComponentModel;

namespace Disqord.Rest.Api
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class RouteExtensions
    {
        public static FormattedRoute Format(this Route route, object[] arguments = null, IEnumerable<KeyValuePair<string, object>> queryParameters = null)
            => RouteFormatter.Format(route, arguments, queryParameters);
    }
}
